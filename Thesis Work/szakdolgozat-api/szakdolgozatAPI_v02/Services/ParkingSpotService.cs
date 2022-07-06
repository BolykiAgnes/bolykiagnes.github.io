using System;
using System.Collections.Generic;
using System.Linq;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.ViewModels;

namespace szakdolgozatAPI_v02.Services
{

    public interface IParkingSpotService
    {
        int SelectSpot(int parkingLotId, string size, DateTime day);
        int GetAvailableSpots(int parkingLotId);
        SpotStatusViewModel IsSpotAvailable(int spotId);
        void RemoveExpiredReservations();
    }
    public class ParkingSpotService : IParkingSpotService
    {
        private IParkingContext dbContext;

        public ParkingSpotService(IParkingContext dbContext)
        {
            this.dbContext = dbContext;
            
        }

        public SpotStatusViewModel IsSpotAvailable(int spotId)
        {
            var spot = this.dbContext.Spots.Find(spotId);
            var reservation = spot.Reservations.Where(x => x.Day.Date == DateTime.Now.Date && x.Status < 2).FirstOrDefault();
            if (spot.VIP)
            {
                return new SpotStatusViewModel
                {
                    SpotId = spotId,
                    Text = "Fenntartott hely",
                    IsAvailable = false
                };
            }
            if (reservation != null)
            {
                return new SpotStatusViewModel
                {
                    SpotId = spotId,
                    Day = reservation.Day,
                    Text = reservation.LicensePlateText,
                    IsAvailable = false
                };
            }
            if (!spot.IsAvailable)
            {
                return new SpotStatusViewModel
                {
                    SpotId = spotId,
                    Text = "Foglalt",
                    IsAvailable = false
                    };
            }
            return new SpotStatusViewModel
            {
                SpotId = spotId,
                Text = "Szabad hely",
                IsAvailable = true
            };
        }

        public int GetAvailableSpots(int parkingLotId)
        {
            int notAvailable = this.dbContext.Spots.Where(x => x.ParkingLotID == parkingLotId && (x.VIP == true || x.IsAvailable == false || x.Reservations.Where(x=>x.Day.Date == DateTime.Now.Date && x.Status < 2).Count() > 0)).Count();
            int allSpots = this.dbContext.Spots.Where(x => x.ParkingLotID == parkingLotId).Count();
            int result = allSpots - notAvailable;
            return result;
        }

        public int SelectSpot(int parkingLotId, string size, DateTime day)
        {
            var reservations = this.dbContext.Reservations.Where(x => x.Day.Date == day.Date && x.Status < 2);
            var spots = size == "X" ? this.dbContext.Spots.Where(x => x.ParkingLotID == parkingLotId && x.VIP == false && x.IsAvailable) : this.dbContext.Spots.Where(x => x.Size == size && x.ParkingLotID == parkingLotId && x.VIP == false && x.IsAvailable);
            foreach (var spot in spots)
            {
                bool available = true;
                foreach (var reservation in reservations)
                {
                    if (reservation.SpotID == spot.SpotID)
                    {
                        available = false;
                    }
                }
                if (available)
                {
                    return spot.SpotID;
                }
            }
            return -1;
        }

        public void RemoveExpiredReservations()
        {
            foreach (var reservation in this.dbContext.Reservations)
            {
                if (reservation.Day.Date < DateTime.Now.Date && reservation.Status == 0)
                {
                    reservation.Status = 3;
                    reservation.Spot.IsAvailable = true;
                }
            }
            this.dbContext.SaveChanges();
        }

      
    }
}
