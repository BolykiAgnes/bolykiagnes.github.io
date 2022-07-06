using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Controllers;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.Hubs;
using szakdolgozatAPI_v02.Models;
using szakdolgozatAPI_v02.Services;
using szakdolgozatAPI_v02.ViewModels;
using Xunit;
using xUnitTests.AsyncQueryProviders;

namespace xUnitTests
{
    public class ParkingLotControllerTests
    {
        Mock<IParkingSpotService> parkingSpotService;
        Mock<IParkingLotService> parkingLotService;
        Mock<IHubContext<NotificationHub>> notificationHub;
        Mock<IParkingContext> dbContext;
        ParkingLotController controller;
        Mock<DbSet<ParkingLot>> mockParkingLots;
        Mock<DbSet<Spot>> mockSpots;

        public ParkingLotControllerTests()
        {
            parkingSpotService = new Mock<IParkingSpotService>();
            parkingLotService = new Mock<IParkingLotService>();
            notificationHub = new Mock<IHubContext<NotificationHub>>();
            dbContext = new Mock<IParkingContext>();
            var fakeParkingLots = new List<ParkingLot> 
            {
                new ParkingLot() {Address = "Test Address 1", Capacity = 25, ParkingLotName = "Test Parking Lot 1", ParkingLotID = 1},
                new ParkingLot() {Address = "Test Address 2", Capacity = 20, ParkingLotName = "Test Parking Lot 2", ParkingLotID = 2},
                new ParkingLot() {Address = "Test Address 3", Capacity = 50, ParkingLotName = "Test Parking Lot 3", ParkingLotID = 3},
                new ParkingLot() {Address = "Test Address 4", Capacity = 5, ParkingLotName = "Test Parking Lot 4", ParkingLotID = 4},
                new ParkingLot() {Address = "Test Address 5", Capacity = 2, ParkingLotName = "Test Parking Lot 5", ParkingLotID = 5},
                new ParkingLot() {Address = "Test Address 6", Capacity = 150, ParkingLotName = "Test Parking Lot 6", ParkingLotID = 6},
                new ParkingLot() {Address = "Test Address 7", Capacity = 550, ParkingLotName = "Test Parking Lot 7", ParkingLotID = 7},
            }.AsQueryable();

            var fakeSpots = new List<Spot>
            {
                new Spot() {ParkingLotID = 1, IsAvailable = true, Size = "M", SpotID = 1, VIP = false },
                new Spot() {ParkingLotID = 1, IsAvailable = true, Size = "M", SpotID = 2, VIP = false },
                new Spot() {ParkingLotID = 1, IsAvailable = true, Size = "M", SpotID = 3, VIP = false },
                new Spot() {ParkingLotID = 2, IsAvailable = true, Size = "M", SpotID = 4, VIP = false },
                new Spot() {ParkingLotID = 3, IsAvailable = true, Size = "M", SpotID = 5, VIP = false },
                new Spot() {ParkingLotID = 4, IsAvailable = true, Size = "M", SpotID = 6, VIP = false },
            }.AsQueryable();

            mockParkingLots = new Mock<DbSet<ParkingLot>>();
            mockSpots = new Mock<DbSet<Spot>>();

            mockSpots.As<IDbAsyncEnumerable<Spot>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Spot>(fakeSpots.GetEnumerator()));
            mockSpots.As<IQueryable<Spot>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Spot>(fakeSpots.Provider));
            mockSpots.As<IQueryable<Spot>>().Setup(m => m.Provider).Returns(fakeSpots.Provider);
            mockSpots.As<IQueryable<Spot>>().Setup(m => m.Expression).Returns(fakeSpots.Expression);
            mockSpots.As<IQueryable<Spot>>().Setup(m => m.ElementType).Returns(fakeSpots.ElementType);
            mockSpots.As<IQueryable<Spot>>().Setup(m => m.GetEnumerator()).Returns(fakeSpots.GetEnumerator());
            mockSpots.Object.AddRange(fakeSpots);
            mockSpots.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(fakeSpots.FirstOrDefault(x => x.SpotID == 1));


            mockParkingLots.As<IDbAsyncEnumerable<ParkingLot>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<ParkingLot>(fakeParkingLots.GetEnumerator()));
            mockParkingLots.As<IQueryable<ParkingLot>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<ParkingLot>(fakeParkingLots.Provider));
            mockParkingLots.As<IQueryable<ParkingLot>>().Setup(m => m.Provider).Returns(fakeParkingLots.Provider);
            mockParkingLots.As<IQueryable<ParkingLot>>().Setup(m => m.Expression).Returns(fakeParkingLots.Expression);
            mockParkingLots.As<IQueryable<ParkingLot>>().Setup(m => m.ElementType).Returns(fakeParkingLots.ElementType);
            mockParkingLots.As<IQueryable<ParkingLot>>().Setup(m => m.GetEnumerator()).Returns(fakeParkingLots.GetEnumerator());
            mockParkingLots.Object.AddRange(fakeParkingLots);




            dbContext.Setup(x => x.ParkingLots).Returns(mockParkingLots.Object);
            dbContext.Setup(x => x.Spots).Returns(mockSpots.Object);
            dbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            controller = new ParkingLotController(dbContext.Object, parkingSpotService.Object, parkingLotService.Object, notificationHub.Object);


        }

        [Fact] 
        public void GetAllParkingLot_Returns_Correct_Number()
        {

            var parkingLotCount = this.dbContext.Object.ParkingLots.ToList().Count;
           


            var result = controller.GetAllParkingLot();

            var parkingLots = ((OkObjectResult)result.Result).Value as IEnumerable<ParkingLotViewModel>;
            var resultCount = (parkingLots as List<ParkingLotViewModel>).Count;

            Assert.Equal(parkingLotCount, resultCount);
            ;


        }

        [Fact]
        public async Task AddParkingLot_Inserts_Record()
        {
            var newParkingLot = new AddParkingLotViewModel()
            {
                Address = "Test Address",
                Name = "Test Parking Lot",
                NumberOfSpots = 500
            };

            var result = await controller.AddParkingLot(newParkingLot);
            var statusCode = (result as ObjectResult)?.StatusCode ?? (result as StatusCodeResult).StatusCode;

            Assert.Equal(200, statusCode);
            this.mockParkingLots.Verify(x => x.AddAsync(It.IsAny<ParkingLot>(), default), Times.Once());
            this.dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once());
            
        }

        [Fact]
        public async void EditParkingLot_Updates_Record()
        {
            var parkingLot = new AddParkingLotViewModel()
            {
                Address = "Test Address",
                Name = "Test Parking Lot 1",
                NumberOfSpots = 2500
            };


            var result = await controller.EditParkingLot(parkingLot);
            var statusCode = (result as StatusCodeResult).StatusCode;

            Assert.Equal(200, statusCode);
            this.mockParkingLots.Verify(x => x.Update(It.IsAny<ParkingLot>()), Times.Once());
            this.dbContext.Verify(x=>x.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public void GetAvailableSpots_Service_Gets_Called()
        {
            int parkingLotId = 1;

            var result = controller.GetAvailableSpots(parkingLotId);

            this.parkingSpotService.Verify(x => x.GetAvailableSpots(parkingLotId), Times.Once());
        }

        [Fact]
        public void GetSpotStatus_Service_Gets_Called()
        {
            int spotId = 1;

            var result = controller.GetSpotStatus(spotId);

            this.parkingSpotService.Verify(x => x.IsSpotAvailable(spotId), Times.Once());
        }


        [Fact]
        public void GetMessage_Service_Gets_Called()
        {
            int parkingLotId = 1;

            var result = controller.GetMessage(parkingLotId);

            this.parkingLotService.Verify(x => x.GetMessage(parkingLotId), Times.Once());
        }

        [Fact]
        public async void ChangeSpotStatus_Updates_Status()
        {
            var model = new ChangeSpotStatusViewModel()
            {
                SpotId = 1,
                IsAvailable = false
            };


            var result = await controller.ChangeSpotStatus(model);
            var statusCode = (result as StatusCodeResult).StatusCode;

            Assert.Equal(200, statusCode);
            dbContext.Verify(x => x.SaveChangesAsync(default), Times.Once());


        }

    }
}
