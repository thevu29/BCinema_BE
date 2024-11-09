using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Roles.Queries
{
    public class GetRolesQuery : IRequest<PaginatedList<RoleDto>>
    {
        public RoleQuery Query { get; init; } = default!;

        public class GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
            : IRequestHandler<GetRolesQuery, PaginatedList<RoleDto>>
        {
            public async Task<PaginatedList<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var query = roleRepository.GetRoles();

                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var roles = await PaginatedList<Role>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var rolesDto = mapper.Map<IEnumerable<RoleDto>>(roles.Data);

                return new PaginatedList<RoleDto>(
                    roles.Page,
                    roles.Size,
                    roles.TotalElements,
                    rolesDto);
            }

            private static IQueryable<Role> ApplySorting(IQueryable<Role> query, string sortBy, string sortOrder)
            {
                var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    nameof(Role.Name),
                    nameof(Role.CreateAt)
                };
                
                if (string.IsNullOrEmpty(sortBy) && !allowedSortColumns.Contains(sortBy))
                {
                    return query.OrderByDescending(r => r.CreateAt);
                }

                return query.ApplyDynamicSorting(sortBy, sortOrder);
            }
        }
    }
}
