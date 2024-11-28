using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class ScheduleRepository(ApplicationDbContext context) : IScheduleRepository
{
    public IQueryable<Schedule> GetSchedules()
    {
        return context.Schedules.AsQueryable().Include(s => s.Room);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(CancellationToken cancellationToken)
    {
        return await context.Schedules
            .Include(s => s.Room)
            .ToListAsync(cancellationToken);
    }

    public async Task<Schedule?> GetScheduleByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Schedules
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByMovieIdAsync(int movieId, CancellationToken cancellationToken)
    {
        return await context.Schedules
            .Include(s => s.Room)
            .Where(s => s.MovieId == movieId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Schedule>> GetSchedulesByRoomAndDateAsync(
        Guid roomId,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        return await context.Schedules
            .Where(s => 
                s.RoomId == roomId && 
                DateOnly.FromDateTime(s.Date) == date)
            .OrderBy(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Schedule> AddScheduleAsync(Schedule schedule, CancellationToken cancellationToken)
    {
        await context.Schedules.AddAsync(schedule, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return schedule;
    }
    
    public async Task AddSchedulesAsync(List<Schedule> schedules, CancellationToken cancellationToken)
    {
        await context.Schedules.AddRangeAsync(schedules, cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
