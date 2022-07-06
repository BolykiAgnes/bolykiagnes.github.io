using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class LicensePlate
    {
        [Key]
        public string LicensePlateText { get; set; }
        public string UserID { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Log> Logs { get; set; }
    }
}
