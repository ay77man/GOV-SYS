using E_Government.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Infrastructure._Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext):base(dbContext) 
        {
            

        }

       
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Meter> Meters { get; set; }

        public DbSet<MeterReading> MetersReading { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Citizens> Citizens { get; set; }
        public DbSet<DrivingLicense> DrivingLicenses { get; set; }
        public DbSet<DrivingLicenseRenewal> DrivingLicenseRenewals { get; set; }
        public DbSet<VehicleLicenseRenewal> VehicleLicenseRenewals { get; set; }

        public DbSet<TrafficViolationPayment> TrafficViolationPayments { get; set; }
        public DbSet<LicenseReplacementRequest> LicenseReplacementRequests { get; set; }

        public DbSet<VehicleOwner> VehicleOwners { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
       .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
