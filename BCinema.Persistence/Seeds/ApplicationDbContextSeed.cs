using System.Security.Cryptography;
using System.Text;
using BCinema.Domain.Entities;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Persistence.Seeds;

public class ApplicationDbContextSeed
{
    public static async Task Seed(ApplicationDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            var adminRoleId = Guid.NewGuid();

            context.Roles.AddRange(
                new Role { Id = adminRoleId, Name = "Admin" },
                new Role { Id = Guid.NewGuid(), Name = "User" }
            );

            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = HashPassword("admin"),
                RoleId = adminRoleId
            });

            context.SeatTypes.Add(new SeatType { Id = Guid.NewGuid(), Name = "Regular", Price = 50 });

            await context.SaveChangesAsync();
        }
    }

    private static string HashPassword(string password)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}