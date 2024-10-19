using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public Guid Id { get; set; }

        public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetRoleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
            {
                var role = await _context.Roles.FindAsync(request.Id)
                    ?? throw new NotFoundException(nameof(Role), request.Id);

                return _mapper.Map<RoleDto>(role);
            }
        }
    }
}
