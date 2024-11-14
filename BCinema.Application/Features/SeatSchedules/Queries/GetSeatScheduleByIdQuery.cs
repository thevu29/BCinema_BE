using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatSchedules.Queries;

public class GetSeatScheduleByIdQuery : IRequest<SeatScheduleDto>
{
    public Guid Id { get; set; }
    
    public class GetSeatScheduleByIdQueryHandler(
        ISeatScheduleRepository seatScheduleRepository,
        IMapper mapper) : IRequestHandler<GetSeatScheduleByIdQuery, SeatScheduleDto>
    {
        public async Task<SeatScheduleDto> Handle(GetSeatScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var seatSchedule = await seatScheduleRepository
                .GetSeatScheduleByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(SeatSchedule));
            
            return mapper.Map<SeatScheduleDto>(seatSchedule);
        }
    }
}