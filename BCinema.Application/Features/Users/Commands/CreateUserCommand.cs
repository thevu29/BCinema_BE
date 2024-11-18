using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Enums;
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

        public class CreateUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper,
            IFileStorageService fileStorageService) : IRequestHandler<CreateUserCommand, UserDto>
        {
            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var role = await roleRepository
                    .GetByIdAsync(request.RoleId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Role));

                
                var user = mapper.Map<User>(request);
                user.Provider = Provider.Local;
                user.Password = passwordHasher.HashPassword(user, request.Password);

                if (request.Avatar != null)
                {
                    await using var imageStream = request.Avatar.OpenReadStream();
                    user.Avatar = await fileStorageService.UploadImageAsync(
                        imageStream, Guid.NewGuid() + ".jpg");
                }

                await userRepository.AddAsync(user, cancellationToken);
                await userRepository.SaveChangesAsync(cancellationToken);

                return mapper.Map<UserDto>(user);
            }
        }
    }
}
