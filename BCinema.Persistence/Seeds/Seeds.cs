﻿using System.Security.Cryptography;
using System.Text;
using BCinema.Domain.Entities;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BCinema.Persistence.Seeds;

public interface IDataSeeder
{
    Task SeedAsync();
    int Order { get; }
}

public class RoleSeeder(ApplicationDbContext context, ILogger<RoleSeeder> logger) : IDataSeeder
{
    public int Order => 1;

    public async Task SeedAsync()
    {
        try
        {
            if (await context.Roles.AnyAsync())
            {
                logger.LogInformation("Roles already seeded");
                return;
            }

            var roles = new List<Role>
            {
                new() { Id = Guid.NewGuid(), Name = "Admin" },
                new() { Id = Guid.NewGuid(), Name = "User" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} roles", roles.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding roles");
            throw;
        }
    }
}

public class UserSeeder(ApplicationDbContext context, ILogger<UserSeeder> logger) : IDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync()
    {
        try
        {
            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Users already seeded");
                return;
            }

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin")
                ?? throw new InvalidOperationException("Admin role not found");

            var users = new List<User>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Password = HashPassword("admin"),
                    RoleId = adminRole.Id
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} users", users.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding users");
            throw;
        }
    }

    private static string HashPassword(string password)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}

public class SeatTypeSeeder(ApplicationDbContext context, ILogger<SeatTypeSeeder> logger)
    : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync()
    {
        try
        {
            if (await context.SeatTypes.AnyAsync())
            {
                logger.LogInformation("Seat types already seeded");
                return;
            }

            var seatTypes = new List<SeatType>
            {
                new() { Id = Guid.NewGuid(), Name = "Regular", Price = 50 },
                new() { Id = Guid.NewGuid(), Name = "VIP", Price = 100 },
                new() { Id = Guid.NewGuid(), Name = "Premium", Price = 150 }
            };

            await context.SeatTypes.AddRangeAsync(seatTypes);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} seat types", seatTypes.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding seat types");
            throw;
        }
    }
}

public class DataSeeder(IServiceProvider serviceProvider, ILogger<DataSeeder> logger)
{
    public async Task SeedAllAsync()
    {
        try
        {
            var seederTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => typeof(IDataSeeder).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
                .ToList();
            
            var seeders = seederTypes
                .Select(seederType => ActivatorUtilities.CreateInstance(serviceProvider, seederType))
                .OfType<IDataSeeder>()
                .ToList();
            
            foreach (var seeder in seeders.OrderBy(s => s.Order))
            {
                logger.LogInformation("Running seeder: {SeederName}", seeder.GetType().Name);
                await seeder.SeedAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding");
            throw;
        }
    }
}

public static class SeederExtensions
{
    public static IServiceCollection AddDataSeeders(this IServiceCollection services)
    {
        services.AddScoped<RoleSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<SeatTypeSeeder>();
        services.AddScoped<DataSeeder>();
        
        return services;
    }
}