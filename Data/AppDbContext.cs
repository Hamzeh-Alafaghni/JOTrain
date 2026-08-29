using JOTrain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JOTrain.Data
{
   
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // These DbSets represent tables that will be created in SQL Server
        public DbSet<User> Users { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // This method configures special rules for the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rule: Prevent SQL Server from crashing due to multiple "Cascade Delete" paths on Trips
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.DepartureStation)
                .WithMany()
                .HasForeignKey(t => t.DepartureStationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.ArrivalStation)
                .WithMany()
                .HasForeignKey(t => t.ArrivalStationId)
                .OnDelete(DeleteBehavior.Restrict);
           

            // 1. Seed Stations
            modelBuilder.Entity<Station>().HasData(
                new Station { Id = 1, Name = "Amman" },
                new Station { Id = 2, Name = "Zarqa" },
                new Station { Id = 3, Name = "Irbid" },
                new Station { Id = 4, Name = "Aqaba" }
            );

            // 2. Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "System Admin", Email = "admin@jotrain.com", Password = "123", Role = UserRole.Admin },
               
                new User { Id = 3, FullName = "Hamzeh", Email = "client@jotrain.com", Password = "123", Role = UserRole.Client }
            );

            // 3. Seed Trips 
            modelBuilder.Entity<Trip>().HasData(
                new Trip
                {
                    Id = 1,
                    DepartureStationId = 1,
                    ArrivalStationId = 2,  
                    DepartureTime = new DateTime(2026, 9, 10, 8, 30, 0),
                    PriceEconomy = 2.00m,
                    PriceEconomyPlus = 3.50m,
                    PriceVIP = 5.00m
                },
                new Trip
                {
                    Id = 2,
                    DepartureStationId = 1, 
                    ArrivalStationId = 3,   
                    DepartureTime = new DateTime(2026, 9, 10, 10, 00, 0),
                    PriceEconomy = 4.00m,
                    PriceEconomyPlus = 6.00m,
                    PriceVIP = 8.00m
                },
                new Trip
                {
                    Id = 3,
                    DepartureStationId = 1, 
                    ArrivalStationId = 4,  
                    DepartureTime = new DateTime(2026, 9, 12, 7, 00, 0),
                    PriceEconomy = 10.00m,
                    PriceEconomyPlus = 15.00m,
                    PriceVIP = 25.00m
                }
                );
        }
    }
}