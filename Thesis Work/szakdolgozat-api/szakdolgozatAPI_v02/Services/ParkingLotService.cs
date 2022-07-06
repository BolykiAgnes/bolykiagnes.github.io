using System.Collections.Generic;
using szakdolgozatAPI_v02.Data;

namespace szakdolgozatAPI_v02.Services
{
    public interface IParkingLotService
    {
        void SetMessage(int parkingLotId, string message);
        string GetMessage(int parkingLotId);
    }
    public class ParkingLotService : IParkingLotService
    {
        private Dictionary<int, string> messages = new Dictionary<int, string>();
        public ParkingLotService()
        {
            this.messages = new Dictionary<int, string>();
        }

        public void SetMessage(int parkingLotId, string message)
        {
            if (this.messages.ContainsKey(parkingLotId))
            {
                this.messages[parkingLotId] = message;
            }
            else
            {
                this.messages.Add(parkingLotId, message);
            }

        }

        public string GetMessage(int parkingLotId)
        {
            if (this.messages.ContainsKey(parkingLotId))
            {
                string result = this.messages[parkingLotId];
                return result;
            }
            return string.Empty;
        }
    }
}
