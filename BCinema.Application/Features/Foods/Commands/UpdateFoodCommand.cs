using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Commands;

public class UpdateFoodCommand : IRequest<FoodDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }

    public class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand, FoodDto>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public UpdateFoodCommandHandler(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await _foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Food));

            _mapper.Map(request, food);

            await _foodRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FoodDto>(food);
        }
    }
}