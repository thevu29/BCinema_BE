using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<PaginatedList<UserDto>>
    {
        public UserQuery Query { get; init; } = default!;

        public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
            : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
        {
            public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var query = userRepository.GetUsers();
                
                if (!string.IsNullOrEmpty(request.Query.Search))
                {
                    var searchTerm = request.Query.Search.Trim().ToLower();
                    query = query.Where(x => 
                        EF.Functions.Like(x.Name.ToLower(), $"%{searchTerm}%") ||
                        EF.Functions.Like(x.Email.ToLower(), $"%{searchTerm}%"));
                }
                if (request.Query.Role.HasValue)
                {
                    query = query.Where(x => x.RoleId == request.Query.Role);
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var users = await PaginatedList<User>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var usersDto = mapper.Map<IEnumerable<UserDto>>(users.Data);

                return new PaginatedList<UserDto>(
                    users.Page,
                    users.Size,
                    users.TotalElements,
                    usersDto);
            }

            private static IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, string sortOrder)
            {
                var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    nameof(User.Name),
                    nameof(User.Email),
                    nameof(User.Point),
                    nameof(Food.CreateAt)
                };
            
                if (string.IsNullOrWhiteSpace(sortBy) || !allowedSortColumns.Contains(sortBy))
                {
                    return query.OrderByDescending(u => u.CreateAt);
                }

                return query.ApplyDynamicSorting(sortBy, sortOrder);
            }
        }
    }
}
