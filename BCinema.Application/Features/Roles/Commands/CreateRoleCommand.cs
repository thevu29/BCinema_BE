﻿using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Roles.Commands
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public CreateFoodCommandHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
            {
                var food = new Food
                {
                    Name = request.Name,
                    Description = request.Description
                };

                _context.Foods.Add(food);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
