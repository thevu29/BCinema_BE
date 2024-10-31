using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Roles.Commands
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;

            public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }

            public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = _mapper.Map<Role>(request);

                await _roleRepository.AddAsync(role, cancellationToken);
                await _roleRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoleDto>(role);
            }
        }
    }
}
