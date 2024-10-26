using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Rooms.Commands
{
    public class DeleteRoomCommand : IRequest<RoomDto>
    {
        public Guid Id { get; set; }

        public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, RoomDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public DeleteRoomCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoomDto> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
            {
                var room = await _context.Rooms.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Room), request.Id);

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}
