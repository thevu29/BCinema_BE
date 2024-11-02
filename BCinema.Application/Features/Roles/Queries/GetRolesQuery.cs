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
        public RoleQuery Query { get; set; } = default!;

        public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PaginatedList<RoleDto>>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;

            public GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedList<RoleDto>> Handle(
                GetRolesQuery request,
                CancellationToken cancellationToken)
            {
                var query = _roleRepository.GetRoles();

                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var roles = await PaginatedList<Role>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(roles.Data);

                return new PaginatedList<RoleDto>(
                    roles.Page,
                    roles.Size,
                    roles.TotalElements,
                    rolesDto);
            }

            private static IQueryable<Role> ApplySorting(
                IQueryable<Role> query,
                string sortBy,
                string sortOrder)
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Name)
                            : query.OrderByDescending(st => st.Name);
                        break;
                    default:
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Id)
                            : query.OrderByDescending(st => st.Id);
                        break;
                }

                return query;
            }
        }
    }
}
