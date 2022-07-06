using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class ParkingLotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int AvailableSpots { get; set; }
    }
}
