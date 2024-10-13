using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Roles.Queries
{
    public class GetRolesQuery : IRequest<IEnumerable<RoleDto>>
    {
        public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync(cancellationToken);
                return _mapper.Map<IEnumerable<RoleDto>>(roles);
            }
        }
    }
}
