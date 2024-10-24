using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Rooms.Queries
{
    public class GetAllRoomQuery : IRequest<IEnumerable<RoomDto>>
    {
        public class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, IEnumerable<RoomDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetAllRoomQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
            {
                var rooms = await _context.Rooms.ToListAsync(cancellationToken);
                return _mapper.Map<IEnumerable<RoomDto>>(rooms);
            }
        }
    }
}
