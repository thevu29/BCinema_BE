using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands
{
    public class UpdateRoomCommand : IRequest<RoomDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public UpdateRoomCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
            {
                var room = await _context.Rooms.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Room), request.Id);

                _mapper.Map(request, room);
                await _context.SaveChangesAsync(cancellationToken);
                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}