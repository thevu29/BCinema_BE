using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Seats.Commands;

public class UpdateSeatCommand : IRequest<SeatDto>
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public Guid? SeatTypeId { get; set; }
    
    public class UpdateSeatCommandHandler : IRequestHandler<UpdateSeatCommand, SeatDto>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        private readonly IMapper _mapper;
        
        public UpdateSeatCommandHandler(
            ISeatRepository seatRepository,
            ISeatTypeRepository seatTypeRepository,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _seatTypeRepository = seatTypeRepository;
            _mapper = mapper;
        }

        public async Task<SeatDto> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
        {
            var seat = await _seatRepository.GetSeatByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Seat));

            if (request.SeatTypeId != null)
            {
                var seatType = await _seatTypeRepository.GetByIdAsync(request.SeatTypeId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(SeatType));
            }

            _mapper.Map<Seat>(request);
            
            await _seatRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<SeatDto>(seat);
        }
    }
}