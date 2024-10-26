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
    public class GetAllSchedulesQuery : IRequest<IEnumerable<ScheduleDto>>
    {
        public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, IEnumerable<ScheduleDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetAllSchedulesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ScheduleDto>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
            {
                var schedules = await _context.Schedules.ToListAsync(cancellationToken);
                return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            }
        }
    }
}
