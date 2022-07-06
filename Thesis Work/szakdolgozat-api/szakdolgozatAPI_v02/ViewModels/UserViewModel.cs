using System.Collections.Generic;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> LicensePlates { get; set; }
    }
}
