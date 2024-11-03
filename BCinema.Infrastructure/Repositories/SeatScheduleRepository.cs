using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class SeatScheduleRepository(ApplicationDbContext context) : ISeatScheduleRepository
{
    public async Task AddSeatSchedulesAsync(IEnumerable<SeatSchedule> seatSchedules, CancellationToken cancellationToken)
    {
        await context.SeatSchedules.AddRangeAsync(seatSchedules, cancellationToken);
    }
    
    public async Task<IEnumerable<SeatSchedule>> GetSeatSchedulesByScheduleIdAsync(Guid scheduleId, CancellationToken cancellationToken)
    {
        return await context.SeatSchedules
            .Where(ss => ss.ScheduleId == scheduleId)
            .Include(ss => ss.Seat)
            .Include(ss => ss.Seat.SeatType)
            .Include(ss => ss.Schedule)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<SeatSchedule?> GetSeatScheduleBySeatIdAndScheduleIdAsync(
        Guid seatId, Guid scheduleId, CancellationToken cancellationToken)
    {
        return await context.SeatSchedules
            .Where(ss => ss.SeatId == seatId && ss.ScheduleId == scheduleId)
            .Include(ss => ss.Seat)
            .Include(ss => ss.Seat.SeatType)
            .Include(ss => ss.Schedule)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<SeatSchedule?> GetSeatScheduleByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.SeatSchedules
            .Include(ss => ss.Seat)
            .Include(ss => ss.Seat.SeatType)
            .Include(ss => ss.Schedule)
            .FirstOrDefaultAsync(ss => ss.Id == id, cancellationToken);
    }
    
    public void DeleteSeatSchedules(IEnumerable<SeatSchedule> seatSchedules)
    {
        context.SeatSchedules.RemoveRange(seatSchedules);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}