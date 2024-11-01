using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Commands;

public class CreateFoodCommand : IRequest<FoodDto>
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public int Quantity { get; set; }

    public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodDto>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public CreateFoodCommandHandler(
            IFoodRepository foodRepository,
            IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = _mapper.Map<Food>(request);

            await _foodRepository.AddAsync(food, cancellationToken);
            await _foodRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FoodDto>(food);
        }
    }
}