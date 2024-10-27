using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface ISeatRepository
{
    IQueryable<Seat> GetSeats();
    Task<IEnumerable<Seat>> GetSeatsAsync(CancellationToken cancellationToken);
    Task<Seat?> GetSeatByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Seat?> GetSeatByRowAndNumberAsync(string row, int number, Guid roomId, CancellationToken cancellationToken);
    Task<IEnumerable<Seat>> GetSeatsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken);
    Task AddSeatAsync(Seat seat, CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}