using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{
    public class GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetUsersAsync(cancellationToken);
            return mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}