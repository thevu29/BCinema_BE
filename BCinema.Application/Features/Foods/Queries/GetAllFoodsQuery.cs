using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Queries;

public class GetAllFoodsQuery : IRequest<IEnumerable<FoodDto>>
{
    public class GetAllFoodsQueryHandler(
        IFoodRepository foodRepository,
        IMapper mapper) : IRequestHandler<GetAllFoodsQuery, IEnumerable<FoodDto>>
    {
        public async Task<IEnumerable<FoodDto>> Handle(GetAllFoodsQuery request, CancellationToken cancellationToken)
        {
            var foods = await foodRepository.GetFoodsAsync(cancellationToken);
            return mapper.Map<IEnumerable<FoodDto>>(foods);
        }
    }
}