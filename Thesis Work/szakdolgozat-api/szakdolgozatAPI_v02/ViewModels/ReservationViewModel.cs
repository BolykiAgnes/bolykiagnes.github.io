using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class ReservationViewModel
    {
        public int ReservationID { get; set; }
        public string UserID { get; set; }
        public int SpotId { get; set; }
        public string ParkingLotName { get; set; }
        public string LicensePlateText { get; set; }
        public string Status { get; set; }
        public DateTime CreateTimeStamp { get; set; }
        public DateTime? ArrivalTimeStamp { get; set; }
        public DateTime? ExitTimeStamp { get; set; }
        public DateTime Day { get; set; }
    }
}
