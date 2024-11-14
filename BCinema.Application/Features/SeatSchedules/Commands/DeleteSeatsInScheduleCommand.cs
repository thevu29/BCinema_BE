using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatSchedules.Commands;

public class DeleteSeatsInScheduleCommand : IRequest
{
    public Guid ScheduleId { get; init; }
    
    public class DeleteSeatsInScheduleCommandHandler(
        ISeatScheduleRepository seatScheduleRepository,
        IScheduleRepository scheduleRepository) : IRequestHandler<DeleteSeatsInScheduleCommand>
    {
        public async Task Handle(DeleteSeatsInScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.ScheduleId, cancellationToken)
                          ?? throw new NotFoundException("Schedule");
            
            if (schedule.Status != Schedule.ScheduleStatus.Ended)
            {
                throw new BadRequestException("Cannot delete seats in ended schedule");
            }
            
            var seatSchedules = await seatScheduleRepository
                .GetSeatSchedulesByScheduleIdAsync(request.ScheduleId, cancellationToken);

            var enumerable = seatSchedules.ToList();
            var seats = enumerable.Where(x => x.Status != SeatSchedule.SeatScheduleStatus.Booked);
            
            seatScheduleRepository.DeleteSeatSchedules(seats);
            await seatScheduleRepository.SaveChangesAsync(cancellationToken);
        }
    }
}