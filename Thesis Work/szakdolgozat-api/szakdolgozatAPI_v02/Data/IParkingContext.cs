using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Models;

namespace szakdolgozatAPI_v02.Data
{
    public interface IParkingContext
    {
        DbSet<ParkingLot> ParkingLots { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<Log> Logs { get; set; }
        DbSet<LicensePlate> LicensePlates { get; set; }
        DbSet<Spot> Spots { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
