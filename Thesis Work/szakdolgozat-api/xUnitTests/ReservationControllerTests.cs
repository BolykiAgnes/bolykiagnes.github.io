using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ReservationControllerTests
    {
        ParkingContext dbContext;
        Mock<IParkingSpotService> parkingSpotService;
        Mock<IParkingLotService> parkingLotService;
        Mock<UserManager<User>> userManager;
        Mock<IHubContext<NotificationHub>> notificationHub;
        ReservationController controller;
        Mock<DbSet<Spot>> mockSpots;
        Mock<DbSet<ParkingLot>> mockParkingLots;
        Mock<DbSet<Reservation>> mockReservations;
        Mock<DbSet<LicensePlate>> mockLicensePlates;
        Mock<DbSet<Log>> mockLogs;
        public ReservationControllerTests()
        {
            parkingLotService = new Mock<IParkingLotService>();
            parkingSpotService = new Mock<IParkingSpotService>();
            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            notificationHub = new Mock<IHubContext<NotificationHub>>();
            var contextOptions = new DbContextOptionsBuilder<ParkingContext>().UseInMemoryDatabase("TestDatabase").UseLazyLoadingProxies().ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            dbContext = new ParkingContext(contextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            var testData = TestDataGenerator.Get();
            this.mockSpots = testData.MockSpots;
            this.mockParkingLots = testData.MockParkingLots;
            this.mockLicensePlates = testData.MockLicensePlates;
            this.mockReservations = testData.MockReservations;
            this.mockLogs = testData.MockLogs;
            dbContext.Spots.AddRange(this.mockSpots.Object);
            dbContext.ParkingLots.AddRange(this.mockParkingLots.Object);
            dbContext.LicensePlates.AddRange(this.mockLicensePlates.Object);
            dbContext.Reservations.AddRange(this.mockReservations.Object);
            dbContext.Logs.AddRange(this.mockLogs.Object);
            dbContext.SaveChanges();

            
            controller = new ReservationController(dbContext, parkingSpotService.Object, parkingLotService.Object, userManager.Object, notificationHub.Object);
        }

        [Fact]
        public void GetAllReservations_Returns_Correct_Number()
        {
            var reservationCount = this.dbContext.Reservations.Where(x => x.Status < 2).ToList().Count();


            var result = controller.GetAllReservation();

            var count = ((OkObjectResult)result.Result).Value as IEnumerable<ReservationViewModel>;

            Assert.Equal(reservationCount, count.Count());
        }

        [Fact]
        public void GetReservationById_Returns_Correct_Value()
        {
            int reservationId = 2;
            var reservation = this.dbContext.Reservations.Where(x => x.ReservationID == reservationId).FirstOrDefault();

            var result = controller.GetReservationById(reservationId);
            var reservationReturned = ((OkObjectResult)result.Result).Value as Reservation;

            Assert.Equal(reservationId, reservationReturned.ReservationID);

        }

        [Fact]
        public void DeleteReservation_Deletes_Record()
        {
            int id = 2;
            int reservationsBeforeDelete = this.dbContext.Reservations.Count();
            
            var result = controller.DeleteReservation(id);

            int reservationsAfterDelete = this.dbContext.Reservations.Count();
            Assert.Equal(reservationsBeforeDelete - 1, reservationsAfterDelete);
        }

        [Fact]
        public void EditReservationStatus_Changes_Status_Correctly()
        {
            var model = new EditReservationStatusViewModel
            {
                ReservationId = 4,
                Status = 2,
            };

            var result = controller.EditReservationStatus(model);
            var reservation = this.dbContext.Reservations.Find(model.ReservationId);
            Assert.Equal(model.Status, reservation.Status);

        }

        

    }
}
