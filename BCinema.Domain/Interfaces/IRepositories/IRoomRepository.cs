using System.Linq.Expressions;
using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface IRoomRepository
{
    IQueryable<Room> GetRooms();
    Task<IEnumerable<Room>> GetRoomsAsync(CancellationToken cancellationToken);
    Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddRoomAsync(Room room, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<Room, bool>> predicate, CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}