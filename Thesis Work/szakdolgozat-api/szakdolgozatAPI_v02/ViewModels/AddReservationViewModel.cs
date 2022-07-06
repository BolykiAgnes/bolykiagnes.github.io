using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class AddReservationViewModel
    {
        public int ParkingLotId { get; set; }
        public string Size { get; set; }
        public DateTime Day { get; set; }
        public string LicensePlateText { get; set; }
    }
}
