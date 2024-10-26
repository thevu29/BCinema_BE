
using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Schedules.Queries
{
    public class GetScheduleByIdQuery : IRequest<ScheduleDto>
    {
        public Guid Id { get; set; }

        public class GetSchedulesByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ScheduleDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSchedulesByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ScheduleDto> Handle(GetScheduleByIdQuery request,  CancellationToken cancellationToken)
            {
                var schedule = await _context.Schedules.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Schedule), request.Id);
                return _mapper.Map<ScheduleDto>(schedule);
            }
        }
    }
}
