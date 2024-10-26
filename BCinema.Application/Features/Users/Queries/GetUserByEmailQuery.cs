using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Users.Queries
{
    public class GetUserByEmailQuery : IRequest<UserDto>
    {
        public string Email { get; set; } = default!;

        public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository
                    .GetByEmailAsync(request.Email, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));

                return _mapper.Map<UserDto>(user);
            }
        }
    }
}
