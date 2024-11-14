using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface ISeatScheduleRepository
{
    Task AddSeatSchedulesAsync(IEnumerable<SeatSchedule> seatSchedules, CancellationToken cancellationToken);
    Task<IEnumerable<SeatSchedule>> GetSeatSchedulesByScheduleIdAsync(Guid scheduleId, CancellationToken cancellationToken);
    Task<SeatSchedule?> GetSeatScheduleBySeatIdAndScheduleIdAsync(Guid seatId, Guid scheduleId, CancellationToken cancellationToken);
    Task<SeatSchedule?> GetSeatScheduleByIdAsync(Guid id, CancellationToken cancellationToken);
    void DeleteSeatSchedules(IEnumerable<SeatSchedule> seatSchedules);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}