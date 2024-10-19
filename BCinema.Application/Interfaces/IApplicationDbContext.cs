using BCinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Food> Foods { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Movie> Movies { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Seat> Seats { get; set; }
        DbSet<SeatType> SeatTypes { get; set; }
        DbSet<MovieGenre> MovieGenres { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<Voucher> Vouchers { get; set; }
        DbSet<UserVoucher> UserVouchers { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<PaymentDetail> PaymentDetails { get; set; }
        DbSet<Token> Tokens { get; set; }

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
