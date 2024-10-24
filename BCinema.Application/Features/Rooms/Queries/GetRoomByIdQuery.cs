using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Rooms.Queries
{
    public class GetRoomByIdQuery : IRequest<RoomDto>
    {
        public Guid Id { get; set; }

        public class GetRoomByIdQueryHandle : IRequestHandler<GetRoomByIdQuery, RoomDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRoomByIdQueryHandle(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
            {
                var room = await _context.Rooms.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Room), request.Id);
                return _mapper.Map<RoomDto>(room);
            }
        }
    }
}
