using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public int SpotID { get; set; }
        public virtual Spot Spot { get; set; }
        public string LicensePlateText { get; set; }
        public virtual LicensePlate LicensePlate { get; set; }
        public int Status { get; set; }
        public DateTime Day {get;set;}
        public DateTime CreateTimeStamp { get; set; }
        public DateTime? ArrivalTimeStamp { get; set; }
        public DateTime? ExitTimeStamp { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
