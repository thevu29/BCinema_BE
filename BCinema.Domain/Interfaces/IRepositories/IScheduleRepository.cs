using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface IScheduleRepository
{
    IQueryable<Schedule> GetSchedules();
    Task<IEnumerable<Schedule>> GetSchedulesAsync(CancellationToken cancellationToken);
    Task<Schedule?> GetScheduleByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Schedule>> GetSchedulesByMovieIdAsync(int movieId, CancellationToken cancellationToken);
    Task<List<Schedule>> GetSchedulesByRoomAndDateAsync(Guid roomId, DateTime date, CancellationToken cancellationToken);
    Task AddScheduleAsync(Schedule schedule, CancellationToken cancellationToken);
    Task AddSchedulesAsync(List<Schedule> schedules, CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}