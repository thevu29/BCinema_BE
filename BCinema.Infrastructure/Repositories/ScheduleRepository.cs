using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly ApplicationDbContext _context;

    public ScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Schedule> GetSchedules()
    {
        return _context.Schedules.AsQueryable().Include(s => s.Room);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(CancellationToken cancellationToken)
    {
        return await _context.Schedules
            .Include(s => s.Room)
            .ToListAsync(cancellationToken);
    }

    public async Task<Schedule?> GetScheduleByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Schedules
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByMovieIdAsync(int movieId, CancellationToken cancellationToken)
    {
        return await _context.Schedules
            .Include(s => s.Room)
            .Where(s => s.MovieId == movieId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Schedule>> GetSchedulesByRoomAndDateAsync(
        Guid roomId,
        DateTime date,
        CancellationToken cancellationToken)
    {
        var startDate = date.Date.ToUniversalTime();
        var endDate = date.Date.AddDays(1).AddTicks(-1).ToUniversalTime();

        return await _context.Schedules
            .Where(s => s.RoomId == roomId && s.Date >= startDate && s.Date <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddScheduleAsync(Schedule schedule, CancellationToken cancellationToken)
    {
        await _context.Schedules.AddAsync(schedule, cancellationToken);
    }
    
    public async Task AddSchedulesAsync(List<Schedule> schedules, CancellationToken cancellationToken)
    {
        await _context.Schedules.AddRangeAsync(schedules, cancellationToken);
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
