using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BCinema.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public IFormFile? Avatar { get; set; }
        public Guid RoleId { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IRoleRepository _roleRepository;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly IMapper _mapper;
            private readonly IFileStorageService _fileStorageService;

            public CreateUserCommandHandler(
                IUserRepository userRepository,
                IRoleRepository roleRepository,
                IPasswordHasher<User> passwordHasher,
                IMapper mapper,
                IFileStorageService fileStorageService)
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
                _fileStorageService = fileStorageService;
            }

            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var role = await _roleRepository
                    .GetByIdAsync(request.RoleId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Role));

                var user = _mapper.Map<User>(request);

                user.Password = _passwordHasher.HashPassword(user, request.Password);

                if (request.Avatar != null)
                {
                    using var imageStream = request.Avatar.OpenReadStream();
                    user.Avatar = await _fileStorageService.UploadImageAsync(
                        imageStream, "avatars/" + Guid.NewGuid() + ".jpg");
                }

                await _userRepository.AddAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<UserDto>(user);
            }
        }
    }
}
