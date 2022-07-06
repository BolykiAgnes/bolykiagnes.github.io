using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Models;
using xUnitTests.AsyncQueryProviders;

namespace xUnitTests
{
    public class TestData
    {
        public Mock<DbSet<Spot>> MockSpots { get; set; }
        public Mock<DbSet<ParkingLot>> MockParkingLots { get; set; }
        public Mock<DbSet<Reservation>> MockReservations { get; set; }
        public Mock<DbSet<LicensePlate>> MockLicensePlates { get; set; }
        public Mock<DbSet<Log>> MockLogs { get; set; }

        public TestData(Mock<DbSet<Spot>> mockSpots, Mock<DbSet<ParkingLot>> mockParkingLots, Mock<DbSet<Reservation>> mockReservations, Mock<DbSet<LicensePlate>> mockLicensePlates, Mock<DbSet<Log>> mockLogs)
        {
            MockSpots = mockSpots;
            MockParkingLots = mockParkingLots;
            MockReservations = mockReservations;
            MockLicensePlates = mockLicensePlates;
            MockLogs = mockLogs;
        }
    }
    public class TestDataGenerator
    {
        public static TestData Get()
        {
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
                new Spot() {ParkingLotID = 4, IsAvailable = true, Size = "M", SpotID = 6, VIP = true },
            }.AsQueryable();

            var fakeLicensePlates = new List<LicensePlate>
            {
                new LicensePlate() {LicensePlateText = "ASD123", CreationDate = DateTime.Now.AddDays(-10), UserID = "asd" },
                new LicensePlate() {LicensePlateText = "FGH456", CreationDate = DateTime.Now.AddDays(-15), UserID = "asd" },
                new LicensePlate() {LicensePlateText = "IJK789", CreationDate = DateTime.Now.AddDays(-17), UserID = "asd" },
            }.AsQueryable();

            var fakeReservations = new List<Reservation>
            {
                new Reservation() { ReservationID = 1, CreateTimeStamp = DateTime.Now.AddDays(-2), Day = DateTime.Now.Date, ArrivalTimeStamp = DateTime.Now.AddHours(-5), ExitTimeStamp = DateTime.Now.AddHours(-2), Status = 2, SpotID = 1, LicensePlateText = "ASD123", UserID = "asd" },
                new Reservation() { ReservationID = 2, CreateTimeStamp = DateTime.Now.AddDays(-3), Day = DateTime.Now.Date.AddDays(-1), ArrivalTimeStamp = DateTime.Now.AddDays(-1).AddHours(-5), ExitTimeStamp = DateTime.Now.AddDays(-1).AddHours(-2), Status = 2, SpotID = 2, LicensePlateText = "IJK789", UserID = "asd" },
                new Reservation() { ReservationID = 3, CreateTimeStamp = DateTime.Now.AddDays(-2), Day = DateTime.Now.Date, Status = 0, SpotID = 3, LicensePlateText = "FGH456", UserID = "asd" },
                new Reservation() { ReservationID = 4, CreateTimeStamp = DateTime.Now.AddDays(-2), Day = DateTime.Now.Date, ArrivalTimeStamp = DateTime.Now.AddHours(-1), Status = 1, SpotID = 1, LicensePlateText = "ASD123", UserID = "asd" }
            }.AsQueryable();

            var fakeLogs = new List<Log>
            {
                new Log() { LogID = 1, CameraID = 0, IsAutomatic = true, FileName = "asd", LicensePlateText = "ASD123", Region = "HU", ReservationID = 1, TimeStamp = DateTime.Now.AddHours(-5)},
                new Log() { LogID = 2, CameraID = 1, IsAutomatic = true, FileName = "asd", LicensePlateText = "ASD123", Region = "HU", ReservationID = 1, TimeStamp = DateTime.Now.AddHours(-2)},
                new Log() { LogID = 3, CameraID = 0, IsAutomatic = true, FileName = "asd", LicensePlateText = "IJK789", Region = "HU", ReservationID = 2, TimeStamp = DateTime.Now.AddDays(-1).AddHours(-5)},
                new Log() { LogID = 4, CameraID = 1, IsAutomatic = true, FileName = "asd", LicensePlateText = "IJK789", Region = "HU", ReservationID = 2, TimeStamp = DateTime.Now.AddDays(-1).AddHours(-2)},
                new Log() { LogID = 5, CameraID = 0, IsAutomatic = true, FileName = "asd", LicensePlateText = "ASD123", Region = "HU", ReservationID = 4, TimeStamp = DateTime.Now.AddHours(-1)},
            }.AsQueryable();

            Mock<DbSet<Spot>> mockSpots = new Mock<DbSet<Spot>>();
            Mock<DbSet<ParkingLot>> mockParkingLots = new Mock<DbSet<ParkingLot>>();
            Mock<DbSet<Reservation>> mockReservations = new Mock<DbSet<Reservation>>();
            Mock<DbSet<LicensePlate>> mockLicensePlates = new Mock<DbSet<LicensePlate>>();
            Mock<DbSet<Log>> mockLogs = new Mock<DbSet<Log>>();

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

            mockLicensePlates.As<IDbAsyncEnumerable<LicensePlate>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<LicensePlate>(fakeLicensePlates.GetEnumerator()));
            mockLicensePlates.As<IQueryable<LicensePlate>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<LicensePlate>(fakeLicensePlates.Provider));
            mockLicensePlates.As<IQueryable<LicensePlate>>().Setup(m => m.Provider).Returns(fakeLicensePlates.Provider);
            mockLicensePlates.As<IQueryable<LicensePlate>>().Setup(m => m.Expression).Returns(fakeLicensePlates.Expression);
            mockLicensePlates.As<IQueryable<LicensePlate>>().Setup(m => m.ElementType).Returns(fakeLicensePlates.ElementType);
            mockLicensePlates.As<IQueryable<LicensePlate>>().Setup(m => m.GetEnumerator()).Returns(fakeLicensePlates.GetEnumerator());
            mockLicensePlates.Object.AddRange(fakeLicensePlates);

            mockReservations.As<IDbAsyncEnumerable<Reservation>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Reservation>(fakeReservations.GetEnumerator()));
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Reservation>(fakeReservations.Provider));
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(fakeReservations.Provider);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(fakeReservations.Expression);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(fakeReservations.ElementType);
            mockReservations.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(fakeReservations.GetEnumerator());
            mockReservations.Object.AddRange(fakeReservations);

            mockLogs.As<IDbAsyncEnumerable<Log>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Log>(fakeLogs.GetEnumerator()));
            mockLogs.As<IQueryable<Log>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Log>(fakeLogs.Provider));
            mockLogs.As<IQueryable<Log>>().Setup(m => m.Provider).Returns(fakeLogs.Provider);
            mockLogs.As<IQueryable<Log>>().Setup(m => m.Expression).Returns(fakeLogs.Expression);
            mockLogs.As<IQueryable<Log>>().Setup(m => m.ElementType).Returns(fakeLogs.ElementType);
            mockLogs.As<IQueryable<Log>>().Setup(m => m.GetEnumerator()).Returns(fakeLogs.GetEnumerator());
            mockLogs.Object.AddRange(fakeLogs);


            return new TestData(mockSpots, mockParkingLots, mockReservations, mockLicensePlates, mockLogs);
        }

    }
}
