using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public Guid Id { get; set; }

        public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
        {
            private readonly IRoleRepository _roleRpository;
            private readonly IMapper _mapper;

            public GetRoleByIdQueryHandler(IRoleRepository roleRpository, IMapper mapper)
            {
                _roleRpository = roleRpository;
                _mapper = mapper;
            }

            public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
            {
                var role = await _roleRpository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(Role));

                return _mapper.Map<RoleDto>(role);
            }
        }
    }
}
