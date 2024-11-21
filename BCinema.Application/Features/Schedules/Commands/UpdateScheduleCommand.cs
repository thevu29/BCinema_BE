using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Schedules.Commands;

public class UpdateScheduleCommand : IRequest<ScheduleDto>
{
    public Guid Id { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan? Time { get; set; }
    public string? Status { get; set; }
    public Guid? RoomId { get; set; }

    public class UpdateScheduleCommandHandler(
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        IMapper mapper)
        : IRequestHandler<UpdateScheduleCommand, ScheduleDto>
    {
        public async Task<ScheduleDto> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Schedule));
            
            if (schedule.Status == Schedule.ScheduleStatus.Ended)
            {
                throw new BadRequestException("Cannot update ended schedule");
            }
            
            if (request.RoomId != null)
            {
                var room = await roomRepository.GetRoomByIdAsync(request.RoomId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(Room));
                
                schedule.Room = room;
            }
            if (request.Date != null)
            {
                var existingTime = schedule.Date.TimeOfDay;
                schedule.Date = DateTime.SpecifyKind(request.Date.Value.Date.Add(existingTime), DateTimeKind.Utc);
            }
            if (request.Time != null)
            {
                schedule.Date = DateTime.SpecifyKind(schedule.Date.Date.Add(request.Time.Value), DateTimeKind.Utc);
            }

            mapper.Map(request, schedule);

            await scheduleRepository.SaveChangesAsync(cancellationToken);
            
            return mapper.Map<ScheduleDto>(schedule);
        }
    }
}