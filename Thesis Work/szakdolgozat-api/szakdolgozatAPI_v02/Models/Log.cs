using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FileName { get; set; }
        public int CameraID { get; set; }
        [Required]
        public string LicensePlateText { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public bool IsAutomatic { get; set; }
        public int? ReservationID { get; set; }
        public virtual LicensePlate LicensePlate { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
