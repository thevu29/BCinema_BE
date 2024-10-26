using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using MediatR;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;

namespace BCinema.Application.Features.Roles.Commands
{
    public class UpdateRoleCommand : IRequest<RoleDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;

            public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }

            public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = await _roleRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(Role));

                _mapper.Map(request, role);

                await _roleRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<RoleDto>(role);
            }
        }
    }
}
