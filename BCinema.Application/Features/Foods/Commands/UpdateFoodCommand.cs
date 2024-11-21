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

    public class UpdateFoodCommandHandler(IFoodRepository foodRepository, IMapper mapper)
        : IRequestHandler<UpdateFoodCommand, FoodDto>
    {
        public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Food));

            mapper.Map(request, food);

            await foodRepository.SaveChangesAsync(cancellationToken);

            return mapper.Map<FoodDto>(food);
        }
    }
}