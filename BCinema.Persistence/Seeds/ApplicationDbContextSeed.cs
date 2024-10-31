using System.Security.Cryptography;
using System.Text;
using BCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Persistence.Seeds;

public class ApplicationDbContextSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var adminRoleId = Guid.NewGuid();
        
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "Admin" },
            new Role { Id = Guid.NewGuid(), Name = "User" }
        );
        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = HashPassword("admin"),
                RoleId = adminRoleId
            }
        );
        
        modelBuilder.Entity<SeatType>().HasData(
            new SeatType { Id = Guid.NewGuid(), Name = "Regular", Price = 50 }
        );
    }
    
    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}