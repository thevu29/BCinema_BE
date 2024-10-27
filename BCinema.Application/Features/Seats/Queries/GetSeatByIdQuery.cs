using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Seats.Queries;

public class GetSeatByIdQuery : IRequest<SeatDto>
{
    public Guid Id { get; set; }

    public class GetSeatByIdQueryHandler : IRequestHandler<GetSeatByIdQuery, SeatDto>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        
        public GetSeatByIdQueryHandler(ISeatRepository seatRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
        }
        
        public async Task<SeatDto> Handle(GetSeatByIdQuery request, CancellationToken cancellationToken)
        {
            var seat = await _seatRepository.GetSeatByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Seat));
            return _mapper.Map<SeatDto>(seat);
        }
    }
}