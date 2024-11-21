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

        public class UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
            : IRequestHandler<UpdateRoleCommand, RoleDto>
        {
            public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                var role = await roleRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(Role));

                mapper.Map(request, role);

                await roleRepository.SaveChangesAsync(cancellationToken);

                return mapper.Map<RoleDto>(role);
            }
        }
    }
}
