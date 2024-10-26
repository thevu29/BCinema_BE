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
        public UserQuery Query { get; set; } = default!;

        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedList<UserDto>> Handle(
                GetUsersQuery request,
                CancellationToken cancellationToken)
            {
                IQueryable<User> query = _userRepository.GetUsers()
                    .Include(u => u.Role);

                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(request.Query.Email))
                {
                    query = query.Where(x => x.Email.ToLower().Contains(request.Query.Email.ToLower()));
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var users = await PaginatedList<User>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var usersDto = _mapper.Map<IEnumerable<UserDto>>(users.Data);

                return new PaginatedList<UserDto>(
                    users.Page,
                    users.Size,
                    users.TotalElements,
                    usersDto);
            }

            private static IQueryable<User> ApplySorting(
                IQueryable<User> query,
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
                    case "email":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Email)
                            : query.OrderByDescending(st => st.Email);
                        break;
                    default:
                        query = query.OrderBy(st => st.Name);
                        break;
                }

                return query;
            }
        }
    }
}
