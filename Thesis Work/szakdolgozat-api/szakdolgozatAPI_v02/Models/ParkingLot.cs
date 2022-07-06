using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class ParkingLot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParkingLotID { get; set; }
        public string ParkingLotName { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Spot> Spots { get; set; }
    }
}
