using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;
using MediatR;

namespace BCinema.Application.Features.Seats.Commands;

public class CreateSeatCommand : IRequest<SeatDto>
{
    public string Row { get; set; } = default!;
    public int Number { get; set; }
    public string? Status { get; set; }
    public Guid SeatTypeId { get; set; }
    public Guid RoomId { get; set; }

    public class CreateSeatCommandHandler : IRequestHandler<CreateSeatCommand, SeatDto>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        
        public CreateSeatCommandHandler(
            ISeatRepository seatRepository,
            ISeatTypeRepository seatTypeRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _seatTypeRepository = seatTypeRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<SeatDto> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
        {
            var seatType = await _seatTypeRepository.GetByIdAsync(request.SeatTypeId, cancellationToken)
                ?? throw new NotFoundException(nameof(SeatType));

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken)
                ?? throw new NotFoundException(nameof(Room));
            
            var existingSeat = await _seatRepository
                .GetSeatByRowAndNumberAsync(request.Row, request.Number, request.RoomId, cancellationToken);
            
            if (existingSeat != null) throw new BadRequestException("Seat already exists");
            
            var seat = _mapper.Map<Seat>(request);

            await _seatRepository.AddSeatAsync(seat, cancellationToken);
            await _seatRepository.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<SeatDto>(seat);
        }
    }
}