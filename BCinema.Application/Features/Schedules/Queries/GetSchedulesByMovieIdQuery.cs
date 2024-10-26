using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Schedules.Queries
{
    public class GetSchedulesByMovieIdQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public string MovieId { get; set; } = default!;

        public class GetSchedulesByMovieIdQueryHandler : IRequestHandler<GetSchedulesByMovieIdQuery, IEnumerable<ScheduleDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSchedulesByMovieIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ScheduleDto>> Handle(GetSchedulesByMovieIdQuery request, CancellationToken cancellationToken)
            {
                    var schedules = await _context.Schedules
                                        .Where(s => s.MovieId == request.MovieId)
                                        .ToListAsync();
                    return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            }
        }
    }
}
