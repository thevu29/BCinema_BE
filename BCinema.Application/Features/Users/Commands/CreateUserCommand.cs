using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Avatar { get; set; }
        public Guid RoleId { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly IMapper _mapper;

            public CreateUserCommandHandler(
                IApplicationDbContext context,
                IPasswordHasher<User> passwordHasher,
                IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == request.RoleId,cancellationToken) 
                    ?? throw new NotFoundException(nameof(Role), request.RoleId);

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Avatar = request.Avatar,
                    RoleId = request.RoleId
                };

                user.Password = _passwordHasher.HashPassword(user, request.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<UserDto>(user);
            }
        }
    }
}
