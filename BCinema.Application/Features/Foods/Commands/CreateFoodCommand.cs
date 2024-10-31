using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.Foods.Commands
{
    public class CreateFoodCommand : IRequest<FoodDto>
    {
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
        public double Price { get; set; }

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
                var food = _mapper.Map<Food>(request);

                _context.Foods.Add(food);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<FoodDto>(food);
            }
        }
    }
}
