using System.Linq.Expressions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyAsync(Expression<Func<Room, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Rooms.AnyAsync(predicate, cancellationToken);
    }
    
    public IQueryable<Room> GetRooms()
    {
        return _context.Rooms.AsQueryable();
    }
    
    public async Task<IEnumerable<Room>> GetRoomsAsync(CancellationToken cancellationToken)
    {
        return await _context.Rooms.ToListAsync(cancellationToken);
    }
    
    public async Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
    
    public async Task AddRoomAsync(Room room, CancellationToken cancellationToken)
    {
        await _context.Rooms.AddAsync(room, cancellationToken);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}