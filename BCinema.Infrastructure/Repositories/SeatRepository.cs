using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;
    
    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Seat> GetSeats()
    {
        return _context.Seats.AsQueryable()
            .Include(s => s.SeatType)
            .Include(s => s.Room);
    }
    
    public async Task<IEnumerable<Seat>> GetSeatsAsync(CancellationToken cancellationToken)
    {
        return await _context.Seats
            .Include(s => s.SeatType)
            .Include(s => s.Room)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Seat>> GetSeatsByRoomIdAsync(Guid roomId, CancellationToken cancellationToken)
    {
        return await _context.Seats
            .Include(s => s.SeatType)
            .Include(s => s.Room)
            .Where(s => s.RoomId == roomId)
            .OrderBy(s => s.Row)
            .ThenBy(s => s.Number)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Seat?> GetSeatByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Seats
            .Include(s => s.SeatType)
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
    
    public async Task<Seat?> GetSeatByRowAndNumberAsync(string row, int number, Guid roomId, CancellationToken cancellationToken)
    {
        return await _context.Seats
            .Include(s => s.SeatType)
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Row == row && s.Number == number && s.RoomId == roomId, cancellationToken);
    }
    
    public async Task AddSeatAsync(Seat seat, CancellationToken cancellationToken)
    {
        await _context.Seats.AddAsync(seat, cancellationToken);
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