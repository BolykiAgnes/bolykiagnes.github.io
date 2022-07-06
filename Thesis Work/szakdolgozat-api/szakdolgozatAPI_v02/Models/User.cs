using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class User :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<LicensePlate> LicensePlates { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
