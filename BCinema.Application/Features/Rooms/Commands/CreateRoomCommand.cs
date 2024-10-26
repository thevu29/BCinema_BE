using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }

        public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateRoomCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
            {
                var room = _mapper.Map<Room>(request);

                _context.Rooms.Add(room);

                foreach (var row in Enumerable.Range(1, request.SeatRows))
                {
                    char rowLabel = (char)('A' + row);

                    foreach (var column in Enumerable.Range(1, request.SeatColumns))
                    {
                        room.Seats.Add(new Seat
                        {
                            Row = rowLabel.ToString(),
                            Number = column
                        });
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}
