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
        public Guid? RoleId { get; set; }
        
        public class UpdateUserCommandHanlder : IRequestHandler<UpdateUserCommand, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IRoleRepository _roleRepository;
            private readonly IFileStorageService _fileStorageService;
            private readonly IMapper _mapper;

            public UpdateUserCommandHanlder(
                IUserRepository userRepository,
                IRoleRepository roleRepository,
                IFileStorageService fileStorageService,
                IMapper mapper)
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _fileStorageService = fileStorageService;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));

                if (request.Avatar != null)
                {
                    using var imageStream = request.Avatar.OpenReadStream();
                    user.Avatar = await _fileStorageService.UploadImageAsync(
                        imageStream, "avatars/" + Guid.NewGuid() + ".jpg");
                }

                if (request.RoleId != null)
                {
                    var role = await _roleRepository.GetByIdAsync(request.RoleId.Value, cancellationToken)
                        ?? throw new NotFoundException(nameof(Role));
                }

                _mapper.Map<User>(request);

                await _userRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<UserDto>(user);
            }
        }
    }   
}
