using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ParkingLotController : ControllerBase
    {
        private readonly IParkingContext dbContext;
        private readonly IParkingSpotService parkingSpotService;
        private readonly IParkingLotService parkingLotService;
        private readonly IHubContext<NotificationHub> notificationHub;

        public ParkingLotController(IParkingContext dbContext, IParkingSpotService parkingSpotService, IParkingLotService parkingLotService, IHubContext<NotificationHub> notificationHub)
        {
            this.dbContext = dbContext;
            this.parkingSpotService = parkingSpotService;
            this.parkingLotService = parkingLotService;
            this.notificationHub = notificationHub;
        }



        [HttpGet("message/{parkingLotId}")]
        public ActionResult GetMessage(int parkingLotId)
        {
            return Ok(new { Message = this.parkingLotService.GetMessage(parkingLotId) });
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParkingLotViewModel>> GetAllParkingLot()
        {
            var ParkingLots = this.dbContext.ParkingLots.OrderBy(p => p.ParkingLotName).ToList();
            List<ParkingLotViewModel> cpvm = new List<ParkingLotViewModel>();
            foreach (var item in ParkingLots)
            {
                cpvm.Add(new ParkingLotViewModel { Id = item.ParkingLotID, Name = item.ParkingLotName, Address = item.Address, AvailableSpots = this.parkingSpotService.GetAvailableSpots(item.ParkingLotID) });
            }
            return Ok(cpvm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddParkingLot([FromBody] AddParkingLotViewModel model) 
        {
            var newParkingLot = new ParkingLot()
            {
                ParkingLotName = model.Name,
                Capacity = model.NumberOfSpots,
                Address = model.Address
            };

            await dbContext.ParkingLots.AddAsync(newParkingLot);
            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                return Ok(newParkingLot);
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> EditParkingLot(AddParkingLotViewModel model)
        {
            var ParkingLot = this.dbContext.ParkingLots.FirstOrDefault(p => p.ParkingLotName == model.Name);
            ParkingLot.ParkingLotName = model.Name;
            ParkingLot.Capacity = model.NumberOfSpots;
            this.dbContext.ParkingLots.Update(ParkingLot);
            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("AvailableSpots/{parkingLotId}")]
        public ActionResult GetAvailableSpots(int parkingLotId)
        {
            int result = this.parkingSpotService.GetAvailableSpots(parkingLotId);
            return Ok(result);
        }


        [HttpGet("Spot/{spotId}")]
        public ActionResult<SpotStatusViewModel> GetSpotStatus(int spotId)
        {
            return Ok(this.parkingSpotService.IsSpotAvailable(spotId));
        }

        [HttpPost("change-status")]
        public async Task<ActionResult> ChangeSpotStatus(ChangeSpotStatusViewModel model)
        {
            var spot = await this.dbContext.Spots.FindAsync(model.SpotId);
            if (spot != null)
            {
                spot.IsAvailable = model.IsAvailable;
            }
            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                if (this.notificationHub.Clients != null && this.notificationHub.Clients.All != null)
                {
                    await this.notificationHub?.Clients?.All?.SendAsync("SpotCalculationNeeded", new { ParkingLotId = spot.ParkingLotID });
                }
               
                return Ok();
            }
            return BadRequest();
        }

    }
}
