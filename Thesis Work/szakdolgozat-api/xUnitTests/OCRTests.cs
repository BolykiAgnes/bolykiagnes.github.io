using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Controllers;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.Hubs;
using szakdolgozatAPI_v02.Models;
using szakdolgozatAPI_v02.Services;
using szakdolgozatAPI_v02.ViewModels;
using Xunit;

namespace xUnitTests
{
    public class OCRTests
    {

        ReservationController controller;

        public OCRTests()
        {
            Mock<IParkingSpotService> parkingSpotService = new Mock<IParkingSpotService>();
            Mock<IParkingLotService> parkingLotService = new Mock<IParkingLotService>();
            Mock<IHubContext<NotificationHub>> notificationHub = new Mock<IHubContext<NotificationHub>>();
            Mock<IParkingContext> dbContext = new Mock<IParkingContext>();
            Mock<UserManager<User>> userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            controller = new ReservationController(dbContext.Object, parkingSpotService.Object, parkingLotService.Object, userManager.Object, notificationHub.Object);
        }

        [Theory]
        [InlineData("OCR/asd123.jpg", "ASD123")]
        [InlineData("OCR/rap235.jpg", "RAP235")]
        [InlineData("OCR/vip.jpg", "ABC123")]
        [InlineData("OCR/nle003.jpg", "NLE003")]
        [InlineData("OCR/ppz489.jpg", "PPZ489")]
        [InlineData("OCR/rvz626.jpg", "RVZ626")]
        public void GetLicensePlate_Returns_Correct_Value(string filePath, string expectedResult)
        {
           
            var file = new FileInfo(Path.Combine(Path.GetFullPath(@"..\..\..\"), filePath));
            string base64String = Convert.ToBase64String(File.ReadAllBytes(file.FullName));
            
            var wb = new WebClient();

            var data = new NameValueCollection();
            string url = "https://api.platerecognizer.com/v1/plate-reader";
            wb.Headers.Add("Authorization", "Token ccaea39ae0b41a1a7ff41562c3a2f4aec2de55b7");
            data["upload"] = base64String;
            System.Threading.Thread.Sleep(3000);
            var response = wb.UploadValues(url, "POST", data);
            string responseAsJson = Encoding.UTF8.GetString(response);
            var responseAsJson2 = JsonConvert.DeserializeObject<PlateRecognizerResponse>(responseAsJson);



            string licensePlate = responseAsJson2.Results.FirstOrDefault()?.Plate.ToUpper();

            Assert.Equal(expectedResult, licensePlate);
        }
    }
}
