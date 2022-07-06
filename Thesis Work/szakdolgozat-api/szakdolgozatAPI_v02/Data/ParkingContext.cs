using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Models;

namespace szakdolgozatAPI_v02.Data
{
    public class ParkingContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IParkingContext
    {
        public virtual DbSet<ParkingLot> ParkingLots { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LicensePlate> LicensePlates { get; set; }
        public virtual DbSet<Spot> Spots { get; set; }


        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reservation>(r => r.HasOne(u => u.User).WithMany(r => r.Reservations).HasForeignKey(r => r.UserID));
            modelBuilder.Entity<Reservation>(r => r.HasOne(s => s.Spot).WithMany(r => r.Reservations).HasForeignKey(r => r.SpotID));
            modelBuilder.Entity<Reservation>(r => r.HasOne(l => l.LicensePlate).WithMany(r => r.Reservations).HasForeignKey(r => r.LicensePlateText));
            modelBuilder.Entity<LicensePlate>(l => l.HasOne(u => u.User).WithMany(l => l.LicensePlates).HasForeignKey(l => l.UserID));
            modelBuilder.Entity<Log>(l => l.HasOne(p => p.LicensePlate).WithMany(l => l.Logs).HasForeignKey(l => l.LicensePlateText));
            modelBuilder.Entity<Log>(l => l.HasOne(r => r.Reservation).WithMany(l => l.Logs).HasForeignKey(l => l.ReservationID));
            modelBuilder.Entity<Spot>(s => s.HasOne(p => p.ParkingLot).WithMany(s => s.Spots).HasForeignKey(s => s.ParkingLotID));

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" },
                new Role { Id = Guid.NewGuid().ToString(), Name = "VIP", NormalizedName = "VIP"}
                );
        }

    }
}
