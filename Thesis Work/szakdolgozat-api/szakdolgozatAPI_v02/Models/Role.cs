using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Services.UserAccountMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Models
{
    public class Role : IdentityRole<string>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
