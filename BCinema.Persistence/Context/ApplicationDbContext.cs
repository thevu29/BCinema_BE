﻿using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using BCinema.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatSchedule> SeatSchedules { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<UserVoucher> UserVouchers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime) || 
                        property.Metadata.ClrType == typeof(DateTime?))
                    {
                        if (property.CurrentValue is DateTime { Kind: DateTimeKind.Local } dateTime)
                        {
                            property.CurrentValue = dateTime.ToUniversalTime();
                        }
                    }
                }
            }
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}