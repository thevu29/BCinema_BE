using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Schedules.Queries
{
    public class GetSchedulesByRoomIdQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public Guid RoomId { get; set; }

        public class GetSchedulesByRoomIdQueryHandler : IRequestHandler<GetSchedulesByRoomIdQuery, IEnumerable<ScheduleDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;   

            public GetSchedulesByRoomIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task <IEnumerable<ScheduleDto>> Handle(GetSchedulesByRoomIdQuery request, CancellationToken cancellationToken)
            {
                var schedules = await _context.Schedules
                    .Where(s => s.RoomId == request.RoomId)
                    .ToListAsync(cancellationToken);

                return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            }
        }
    }
}
