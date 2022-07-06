using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class Spot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpotID { get; set; }
        public int ParkingLotID { get; set; }
        public string Size { get; set; }
        public bool VIP { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ParkingLot ParkingLot { get; set; }
    }
}
