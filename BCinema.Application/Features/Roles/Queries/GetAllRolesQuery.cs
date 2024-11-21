using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Roles.Queries;

public class GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>
{
    public class GetAllRolesQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper) : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
    {
        public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await roleRepository.GetRolesAsync(cancellationToken);
            return mapper.Map<IEnumerable<RoleDto>>(roles);
        }
    }
}