using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Roles.Commands
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateRoleCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = new Role
                {
                    Name = request.Name,
                    Description = request.Description
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoleDto>(role);
            }
        }
    }
}
