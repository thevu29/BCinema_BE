using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Application.Exceptions;
using MediatR;

namespace BCinema.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }

        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));

                return _mapper.Map<UserDto>(user);
            }
        }
    }
}
