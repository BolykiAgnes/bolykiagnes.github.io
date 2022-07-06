using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.Hubs;
using szakdolgozatAPI_v02.Models;
using szakdolgozatAPI_v02.Services;
using szakdolgozatAPI_v02.ViewModels;

namespace szakdolgozatAPI_v02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IParkingContext dbContext;
        private readonly IParkingSpotService parkingSpotService;
        private readonly IParkingLotService parkingLotService;
        private readonly IHubContext<NotificationHub> notificationHub;
        public ReservationController(IParkingContext dbContext, IParkingSpotService parkingSpotService, IParkingLotService parkingLotService, UserManager<User> userManager, IHubContext<NotificationHub> notificationHub)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.parkingSpotService = parkingSpotService;
            this.parkingLotService = parkingLotService;
            this.parkingSpotService.RemoveExpiredReservations();
            this.notificationHub = notificationHub;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReservationViewModel>> GetAllReservation() 
        {
            var reservations = this.dbContext.Reservations.OrderBy(p => p.CreateTimeStamp).Where(x => x.Status < 2).ToList();
            var result = new List<ReservationViewModel>();
            foreach (var reservation in reservations)
            {
                result.Add(new ReservationViewModel()
                {
                    ReservationID = reservation.ReservationID,
                    SpotId = reservation.SpotID,
                    UserID = reservation.UserID,
                    Status = ConvertStatusCode(reservation.Status),
                    LicensePlateText = reservation.LicensePlateText,
                    ParkingLotName = reservation.Spot.ParkingLot.ParkingLotName,
                    ExitTimeStamp = reservation.ExitTimeStamp,
                    ArrivalTimeStamp = reservation.ArrivalTimeStamp,
                    CreateTimeStamp = reservation.CreateTimeStamp,
                    Day = reservation.Day
                });
            }
            return Ok(result.OrderByDescending(x=>x.Day));
        }

        [HttpGet("user")]
        public ActionResult<IEnumerable<ReservationViewModel>> GetAllReservationByUser() 
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var reservations = this.dbContext.Reservations.Where(p => p.UserID == userId).ToList();
            var result = new List<ReservationViewModel>();
            foreach (var reservation in reservations)
            {
                result.Add(new ReservationViewModel()
                {
                    ReservationID = reservation.ReservationID,
                    SpotId = reservation.SpotID,
                    UserID = userId,
                    Status = ConvertStatusCode(reservation.Status),
                    LicensePlateText = reservation.LicensePlateText,
                    ParkingLotName = reservation.Spot.ParkingLot.ParkingLotName,
                    ExitTimeStamp = reservation.ExitTimeStamp,
                    ArrivalTimeStamp = reservation.ArrivalTimeStamp,
                    CreateTimeStamp = reservation.CreateTimeStamp,
                    Day = reservation.Day
                });
            }
            return Ok(result.OrderByDescending(x => x.Day));
        }

        private string ConvertStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 0:
                    return "Foglalt";
                case 1:
                    return "Folyamatban";
                case 2:
                    return "Befejezett";
                case 3:
                    return "Lejárt";
                default: 
                    return "Érvénytelen";

            }
        }

        [HttpGet("reservation/{id}")]
        public ActionResult<Reservation> GetReservationById(int id)
        {
            var reservations = this.dbContext.Reservations.Where(p => p.ReservationID == id).FirstOrDefault();
            return Ok(reservations);
        }

        [HttpGet("parkingLot/{id}")]
        public async Task<ActionResult> GetReservationsByParkingLot(int id) 
        {
            var parkingLot = await this.dbContext.ParkingLots.FirstAsync(p => p.ParkingLotID == id);
            var reservations = parkingLot.Reservations.ToList();
            return Ok(reservations);
        }

        [HttpPost("alpr")]
        public async Task<ActionResult> SendImage([FromForm]CameraInputViewModel model)
        {
            string base64String = "";
            using (MemoryStream m = new MemoryStream())
            {
                model.Image.CopyTo(m);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);

            }
            var wb = new WebClient();

            var data = new NameValueCollection();
            string url = "https://api.platerecognizer.com/v1/plate-reader";
            wb.Headers.Add("Authorization", "Token [token]");
            data["upload"] = base64String;
            data["camera_id"] = model.CameraID.ToString();
            var response = wb.UploadValues(url, "POST", data);
            string responseAsJson = Encoding.UTF8.GetString(response);
            var responseAsJson2 = JsonConvert.DeserializeObject<PlateRecognizerResponse>(responseAsJson);

            var licensePlateText = responseAsJson2.Results.Count > 0 ? responseAsJson2.Results[0].Plate.ToUpper() : "N/A";
            var originalLicensePlateText = licensePlateText;
            var availableSpots = this.parkingSpotService.GetAvailableSpots(model.ParkingLotId); 

            var reservation = await this.dbContext.Reservations.Where(x => x.LicensePlateText == licensePlateText && x.Day.Date == DateTime.Now.Date && x.Spot.ParkingLotID == model.ParkingLotId && x.Status < 2).FirstOrDefaultAsync();
            var licensePlate = await this.dbContext.LicensePlates.FindAsync(licensePlateText);
            if (licensePlate == null)
            {
                bool found = false;
                int i = 0;
                while (!found && responseAsJson2.Results.Count > 0 && i < responseAsJson2.Results?.First().Candidates.Count)
                {
                    licensePlateText = responseAsJson2.Results.First().Candidates[i].Plate.ToUpper();
                    reservation = await this.dbContext.Reservations.Where(x => x.LicensePlateText == licensePlateText && x.Day.Date == DateTime.Now.Date && x.Spot.ParkingLotID == model.ParkingLotId && x.Status < 2).FirstOrDefaultAsync();
                    licensePlate = await this.dbContext.LicensePlates.FindAsync(licensePlateText);
                    if (licensePlate != null)
                    {
                        found = true;
                    }
                    i++;
                }
            }

            var newLog = new Log()
            {
                TimeStamp = responseAsJson2.Timestamp,
                FileName = responseAsJson2.Filename,
                LicensePlateText = licensePlate?.LicensePlateText ?? originalLicensePlateText,
                Region = responseAsJson2.Results.Count > 0 ? responseAsJson2.Results[0].Region.Code : "N/A",
                Type = responseAsJson2.Results.Count > 0 ? responseAsJson2.Results[0].Vehicle.Type : "N/A",
                CameraID = model.CameraID,
                ReservationID = reservation?.ReservationID ?? null,
                IsAutomatic = true,

            };

            var user = this.dbContext.LicensePlates.Find(newLog.LicensePlateText)?.User;
            bool allowed = false;

            if (model.CameraID == 0)
            {
                if (reservation != null)
                {
                    reservation.Status = 1;
                    reservation.Spot.IsAvailable = false;
                    reservation.ArrivalTimeStamp = DateTime.Now;
                    allowed = true;
                }
                else
                {
                    if (availableSpots > 0)
                    {
                        allowed = true;
                    }
                }
            }
            else
            {
                if (reservation != null)
                {
                    reservation.Status = 2;
                    reservation.Spot.IsAvailable = true;
                    reservation.ExitTimeStamp = DateTime.Now;
                }
            }
            await dbContext.Logs.AddAsync(newLog);
            if (await dbContext.SaveChangesAsync() > 0)
            {
                if (model.CameraID == 0)
                {
                    if (user != null && await this.userManager.IsInRoleAsync(user, "VIP"))
                    {
                        string message = "Ön rendelkezik fenntartott parkolóval, így behajthat.";
                        this.parkingLotService.SetMessage(model.ParkingLotId, message);
                        await this.notificationHub.Clients.All.SendAsync("EntranceMessageReceived", new { ParkingLotId = model.ParkingLotId, Message = message });
                        return Ok(message);
                    }
                    
                    if (allowed)
                    {
                        if (reservation != null)
                        {
                            string message = "Parkolóhelyének száma: " + reservation.SpotID;
                            this.parkingLotService.SetMessage(model.ParkingLotId, message);
                            await this.notificationHub.Clients.All.SendAsync("EntranceMessageReceived", new { ParkingLotId = model.ParkingLotId, Message = message });
                            return Ok(message);
                        }
                        else
                        {
                            string message = "Behajthat, még ennyi szabad hely van: " + availableSpots;
                            this.parkingLotService.SetMessage(model.ParkingLotId, message);
                            await this.notificationHub.Clients.All.SendAsync("EntranceMessageReceived", new { ParkingLotId = model.ParkingLotId, Message = message });
                            return Ok(message);
                        }
                    }
                    else
                    {
                        string message = "Sajnos jelenleg nincs szabad hely.";
                        this.parkingLotService.SetMessage(model.ParkingLotId, message);
                        await this.notificationHub.Clients.All.SendAsync("EntranceMessageReceived", new { ParkingLotId = model.ParkingLotId, Message = message });
                        return BadRequest(message);
                    }
                }
                await this.notificationHub.Clients.All.SendAsync("VehicleExited", new { ParkingLotId = model.ParkingLotId});
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> AddReservation([FromBody] AddReservationViewModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var spotId = this.parkingSpotService.SelectSpot(model.ParkingLotId, model.Size, model.Day);
            if (spotId == -1)
            {
                return BadRequest("A megadott napra nincs a feltételeknek megfelelő szabad parkolóhely.");
            }
            
            bool activeReservation = this.dbContext.Reservations.Where(x => x.LicensePlateText == model.LicensePlateText && model.Day.Date == x.Day.Date && x.Status < 2).Count() > 0;

            if (activeReservation)
            {
                return BadRequest("A megadott napra már van nem befejezett foglalás a megadott autóra!");
            }

            var reservation = new Reservation()
            {
                UserID = userId,
                SpotID = spotId,
                LicensePlateText = model.LicensePlateText,
                Status = 0,
                CreateTimeStamp = DateTime.Now,
                ArrivalTimeStamp = null,
                ExitTimeStamp = null,
                Day = model.Day
            };
            
            await this.dbContext.Reservations.AddAsync(reservation);
            if(await this.dbContext.SaveChangesAsync() > 0)
            {
                await this.notificationHub.Clients.All.SendAsync("SpotCalculationNeeded", new { ParkingLotId = model.ParkingLotId});
                return Ok(new { reservation.SpotID, reservation.Spot.Size });
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditReservation(EditReservationViewModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var reservation = await this.dbContext.Reservations.FindAsync(model.ReservationID);
            if (reservation == null)
            {
                return BadRequest("Nem létezik foglalás a megadott azonosítóval.");
            }
            if (reservation.UserID != userId)
            {
                return Unauthorized("Csak a saját foglalásod módosíthatod.");
            }
            if (reservation.Status > 0)
            {
                return Unauthorized("Csak még meg nem kezdett parkolást lehet módosítani!");
            }
            if (reservation.LicensePlateText != model.LicensePlateText)
            {
                reservation.LicensePlateText = model.LicensePlateText;
            }
            if (reservation.Day != model.Day)
            {
               
                var size = reservation.Spot.Size;
                var parkingLotId = reservation.Spot.ParkingLotID;
                var newSpotId = this.parkingSpotService.SelectSpot(parkingLotId, size, model.Day);
                if (newSpotId == -1)
                {
                    return BadRequest("A megadott napra nincs a feltételeknek megfelelő szabad parkolóhely.");
                }
                reservation.Day = model.Day;
                reservation.SpotID = newSpotId;
            }

            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                await this.notificationHub.Clients.All.SendAsync("SpotCalculationNeeded", new { ParkingLotId = reservation.Spot.ParkingLotID });
                return Ok(reservation.SpotID);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> CancelReservation(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var reservation = this.dbContext.Reservations.FirstOrDefault(r => r.ReservationID == id);
            
            if (reservation == null)
            {
                return BadRequest("A megadott foglalás azonosító nem létezik.");
            }
            var parkingLotId = reservation.Spot.ParkingLotID;
            if (reservation.UserID != userId)
            {
                return Unauthorized("Nincs jogosultságod a foglalás törléséhez!");
            }
            if (reservation.Status > 0)
            {
                return Unauthorized("Csak még meg nem kezdett parkolást lehet törölni!");
            }
            this.dbContext.Reservations.Remove(reservation);
            await this.dbContext.SaveChangesAsync();
            await this.notificationHub.Clients.All.SendAsync("SpotCalculationNeeded", new { ParkingLotId = parkingLotId });
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);
            var parkingLotId = reservation.Spot.ParkingLotID;
            if (reservation == null)
            {
                return BadRequest("A megadott foglalás azonosító nem létezik.");
            }
            this.dbContext.Reservations.Remove(reservation);
            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                if (this.notificationHub.Clients != null && this.notificationHub.Clients.All != null)
                {
                    await this.notificationHub.Clients.All.SendAsync("SpotCalculationNeeded", new { ParkingLotId = parkingLotId });
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("editStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditReservationStatus(EditReservationStatusViewModel model)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(model.ReservationId);
            if (reservation == null)
            {
                return BadRequest("A megadott foglalás azonosító nem létezik.");
            }
            
            if (model.Status == 1)
            {
                reservation.Status = 1;
                reservation.ArrivalTimeStamp = DateTime.Now;
                var log = new Log()
                {
                    CameraID = -1,
                    IsAutomatic = false,
                    LicensePlateText = reservation.LicensePlateText,
                    ReservationID = reservation.ReservationID,
                    TimeStamp = DateTime.Now
                };
                reservation.Logs.Add(log);
            }
            else
            {
                reservation.Status = 2;
                reservation.ExitTimeStamp = DateTime.Now;
                var log = new Log()
                {
                    CameraID = -2,
                    IsAutomatic = false,
                    LicensePlateText = reservation.LicensePlateText,
                    ReservationID = reservation.ReservationID,
                    TimeStamp = DateTime.Now
                };
                reservation.Logs.Add(log);
            }

            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                if (this.notificationHub.Clients != null && this.notificationHub.Clients.All != null)
                {
                    await this.notificationHub.Clients.All.SendAsync("SpotCalculationNeeded", new { ParkingLotId = reservation.Spot.ParkingLotID });
                }
                
                return Ok();
            }
            return BadRequest();

        }

       
    }
}
