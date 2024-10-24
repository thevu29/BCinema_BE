using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Foods.Queries
{
    public class GetFoodByNameQuery : IRequest<FoodDto>
    {
        public string Name { get; set; } = default!;
        public class GetFoodByNameQueryHandle : IRequestHandler<GetFoodByNameQuery, FoodDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetFoodByNameQueryHandle(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<FoodDto> Handle(GetFoodByNameQuery request, CancellationToken cancellationToken)
            {
                var food = await _context.Foods
                    .FirstOrDefaultAsync(v => v.Name == request.Name, cancellationToken: cancellationToken)
                    ?? throw new NotFoundException(nameof(Foods), request.Name);
                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
