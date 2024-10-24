using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

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
                var room = new Room
                {
                    Name = request.Name,
                    Description = request.Description
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}
