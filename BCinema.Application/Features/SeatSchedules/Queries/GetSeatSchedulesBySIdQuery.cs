using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatSchedules.Queries;

public class GetSeatSchedulesBySIdQuery : IRequest<IEnumerable<SeatScheduleDto>>
{
    public Guid ScheduleId { get; set; }
    
    public class GetSeatSchedulesBySIdQueryHandler(
        ISeatScheduleRepository seatScheduleRepository,
        IScheduleRepository scheduleRepository,
        IMapper mapper) : IRequestHandler<GetSeatSchedulesBySIdQuery, IEnumerable<SeatScheduleDto>>
    {
        public async Task<IEnumerable<SeatScheduleDto>> Handle(GetSeatSchedulesBySIdQuery request, CancellationToken cancellationToken)
        {
            var schedule = await scheduleRepository
                .GetScheduleByIdAsync(request.ScheduleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Schedule));
            
            var seatSchedules = await seatScheduleRepository
                .GetSeatSchedulesByScheduleIdAsync(schedule.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(SeatSchedule));
            
            return mapper.Map<IEnumerable<SeatScheduleDto>>(seatSchedules);
        }
    }
}