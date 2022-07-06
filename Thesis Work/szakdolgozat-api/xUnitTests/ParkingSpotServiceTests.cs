using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.Services;
using Xunit;

namespace xUnitTests
{
    
    public class ParkingSpotServiceTests
    {
        ParkingContext dbContext;
        IParkingSpotService parkingSpotService;
        public ParkingSpotServiceTests()
        {
            var testData = TestDataGenerator.Get();
            var mockSpots = testData.MockSpots;
            var mockParkingLots = testData.MockParkingLots;
            var mockLicensePlates = testData.MockLicensePlates;
            var mockReservations = testData.MockReservations;
            var mockLogs = testData.MockLogs;
            var contextOptions = new DbContextOptionsBuilder<ParkingContext>().UseInMemoryDatabase("TestDatabase").UseLazyLoadingProxies().ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            dbContext = new ParkingContext(contextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Spots.AddRange(mockSpots.Object);
            dbContext.ParkingLots.AddRange(mockParkingLots.Object);
            dbContext.LicensePlates.AddRange(mockLicensePlates.Object);
            dbContext.Reservations.AddRange(mockReservations.Object);
            dbContext.Logs.AddRange(mockLogs.Object);
            dbContext.SaveChanges();
            this.parkingSpotService = new ParkingSpotService(dbContext);

        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public void IsSpotAvailable_Returns_Correct_Value(int spotId, bool isAvailable)
        {

            var result = this.parkingSpotService.IsSpotAvailable(spotId);

            Assert.Equal(isAvailable, result.IsAvailable);

        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 1)]
        [InlineData(4, 0)]
        public void GetAvailableSpots_Returns_Correct_Value(int parkingLotId, int numberOfAvailable)
        {
            var result = parkingSpotService.GetAvailableSpots(parkingLotId);

            Assert.Equal(numberOfAvailable, result);
        }


        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(2, 0, 4)]
        [InlineData(1, 1, 1)]
        public void SelectSpot_Returns_Correct_Value(int parkingLotId, int daysFromNow, int expectedSpotId)
        {
            var result = parkingSpotService.SelectSpot(parkingLotId, "M", DateTime.Now.Date.AddDays(daysFromNow));

            Assert.Equal(expectedSpotId, result);
        }


    }
}
