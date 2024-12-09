using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Queries;

public class GetFoodByIdQuery : IRequest<FoodDto>
{
    public Guid Id { get; set; }

    public class GetFoodByIdQueryHandler(IFoodRepository foodRepository, IMapper mapper)
        : IRequestHandler<GetFoodByIdQuery, FoodDto>
    {
        public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
        {
            var food = await foodRepository.GetFoodByIdAsync(request.Id, cancellationToken)
                       ?? throw new NotFoundException(nameof(Food));

            return mapper.Map<FoodDto>(food);
        }
    }
}