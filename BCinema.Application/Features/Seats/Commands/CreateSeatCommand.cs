using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Seats.Commands
{
    public class CreateSeatCommand : IRequest<SeatDto>
    {
        public string Row { get; set; } = default!;
        public int Number { get; set; } = default!;
        public SeatStatus SeatStatus { get; set; } = SeatStatus.Available;
        public Guid SeatTypeId { get; set; }
        public Guid RoomId { get; set; }

        public class CreateSeatCommandHandler : IRequestHandler<CreateSeatCommand, SeatDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateSeatCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SeatDto> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
            {
                var seatType = _context.SeatTypes.FirstOrDefault(x => x.Id == request.SeatTypeId);
                if (seatType == null)
                {
                    throw new NotFoundException(nameof(SeatType), request.SeatTypeId);
                }

                var room = _context.Rooms.FirstOrDefault(x => x.Id == request.RoomId);
                if (room == null)
                {
                    throw new NotFoundException(nameof(Room), request.RoomId);
                }

                if (_context.Seats.Any(x => x.Row == request.Row && x.Number == request.Number && x.RoomId == request.RoomId))
                {
                    throw new BadRequestException("Seat already exists");
                }

                var seat = _mapper.Map<Seat>(request);
                _context.Seats.Add(seat);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SeatDto>(seat);
            }
        }
    }

}
