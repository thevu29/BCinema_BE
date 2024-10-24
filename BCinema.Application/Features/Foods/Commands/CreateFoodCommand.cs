using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Roles.Commands;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Foods.Commands
{
    public class CreateFoodCommand : IRequest<FoodDto>
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
