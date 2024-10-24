using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Roles.Queries;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Foods.Queries
{
    public class GetFoodByIdQuery : IRequest<FoodDto>
    {
        public Guid Id { get; set; }

        public class GetFoodByIdQueryHandler : IRequestHandler<GetFoodByIdQuery, FoodDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetFoodByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
            {
                var food = await _context.Roles.FindAsync(request.Id)
                    ?? throw new NotFoundException(nameof(Food), request.Id);

                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
