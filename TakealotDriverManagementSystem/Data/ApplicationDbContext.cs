using Microsoft.EntityFrameworkCore;
using TakealotDriverManagementSystem.Models;

namespace TakealotDriverManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // DbSet properties for entities
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<JobApplication> Applications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Driver) // User -> Driver
                .WithOne(d => d.User) // Driver -> User
                .HasForeignKey<Driver>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User) // Notification -> User
                .WithMany(u => u.Notifications) // User -> Notification
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.User) // ActivityLog -> User
                .WithMany(u => u.ActivityLogs) // User -> ActivityLog
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(j => j.User) // JobApplication -> User
                .WithMany(u => u.JobApplications) // User -> JobApplication
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.AssignedVehicle) // Driver -> AssignedVehicle
                .WithOne(v => v.Driver) // AssignedVehicle -> Driver
                .HasForeignKey<Driver>(d => d.AssignedVehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Warehouse) // Vehicle -> Warehouse
                .WithMany(w => w.Vehicles) // Warehouse -> Vehicle
                .HasForeignKey(v => v.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Warehouse) // Vacancy -> Warehouse
                .WithMany(w => w.Vacancies) // Warehouse -> Vacancy
                .HasForeignKey(v => v.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(j => j.Vacancy) // JobApplication -> Vacancy
                .WithMany(v => v.Applications) // Vacancy -> JobApplications
                .HasForeignKey(j => j.VacancyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure User.Email is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ensure Vehicle.LicensePlate is unique
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate)
                .IsUnique();
        }
    }
}
