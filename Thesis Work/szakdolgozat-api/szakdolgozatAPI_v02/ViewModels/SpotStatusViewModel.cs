using System;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class SpotStatusViewModel
    {
        public int SpotId { get; set; }
        public bool IsAvailable { get; set; }
        public string Text { get; set; }
        public DateTime Day { get; set; }
    }
}
