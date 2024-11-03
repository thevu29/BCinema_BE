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

    public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, ScheduleDto>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        
        public UpdateScheduleCommandHandler(
            IScheduleRepository scheduleRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<ScheduleDto> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Schedule));
            
            if (schedule.Status == Domain.Entities.Schedule.ScheduleStatus.Ended)
            {
                throw new BadRequestException("Cannot update ended schedule");
            }
            
            if (request.RoomId != null)
            {
                var room = await _roomRepository.GetRoomByIdAsync(request.RoomId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(Room));
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

            _mapper.Map(request, schedule);

            await _scheduleRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<ScheduleDto>(schedule);
        }
    }
}