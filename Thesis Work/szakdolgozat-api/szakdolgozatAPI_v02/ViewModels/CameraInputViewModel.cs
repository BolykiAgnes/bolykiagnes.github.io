using Microsoft.AspNetCore.Http;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class CameraInputViewModel
    {
        public IFormFile Image { get; set; }
        public int CameraID { get; set; }
        public int ParkingLotId { get; set; }
    }
}
