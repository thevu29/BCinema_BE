using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatSchedules.Queries;

public class GetSeatScheduleBySdIdAndSId : IRequest<SeatScheduleDto>
{
    public Guid ScheduleId { get; set; }
    public Guid SeatId { get; set; }
    
    public class GetSeatScheduleBySdIdAndSIdHandler(
        ISeatScheduleRepository seatScheduleRepository,
        IScheduleRepository scheduleRepository,
        ISeatRepository seatRepository,
        IMapper mapper) : IRequestHandler<GetSeatScheduleBySdIdAndSId, SeatScheduleDto>
    {
        public async Task<SeatScheduleDto> Handle(GetSeatScheduleBySdIdAndSId request, CancellationToken cancellationToken)
        {
            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.ScheduleId, cancellationToken) 
                           ?? throw new NotFoundException(nameof(Schedule));
            
            var seat = await seatRepository.GetSeatByIdAsync(request.SeatId, cancellationToken) 
                       ?? throw new NotFoundException(nameof(Seat));
            
            var seatSchedule = await seatScheduleRepository
                .GetSeatScheduleBySeatIdAndScheduleIdAsync(seat.Id, schedule.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(SeatSchedule));
            
            return mapper.Map<SeatScheduleDto>(seatSchedule);
        }
    } 
}