using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Interfaces.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? Avatar { get; set; }
        
        public class UpdateUserCommandHandler(
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            IMapper mapper) : IRequestHandler<UpdateUserCommand, UserDto>
        {
            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await userRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));

                if (request.Avatar != null)
                {
                    await using var imageStream = request.Avatar.OpenReadStream();
                    
                    user.Avatar = !string.IsNullOrEmpty(user.Avatar) 
                        ? await fileStorageService.UpdateImageAsync(imageStream, Guid.NewGuid() + ".jpg", user.Avatar)
                        : await fileStorageService.UploadImageAsync(imageStream, Guid.NewGuid() + ".jpg");
                }
                
                mapper.Map<User>(request);

                await userRepository.SaveChangesAsync(cancellationToken);

                return mapper.Map<UserDto>(user);
            }
        }
    }   
}
