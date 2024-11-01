using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Foods.Queries;

public class GetFoodsQuery : IRequest<PaginatedList<FoodDto>>
{
    public FoodQuery Query { get; set; } = default!;

    public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, PaginatedList<FoodDto>>
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public GetFoodsQueryHandler(IFoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<FoodDto>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
        {
            var query = _foodRepository.GetFoods();

            if (!string.IsNullOrEmpty(request.Query.Name))
            {
                query = query.Where(f => f.Name.ToLower().Contains(request.Query.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Query.Price))
            {
                query = FilterByNumber(query, request.Query.Price, "price");
            }
            if (!string.IsNullOrEmpty(request.Query.Quantity))
            {
                query = FilterByNumber(query, request.Query.Quantity, "quantity");
            }

            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

            var foods = await PaginatedList<Food>.ToPageList(query, request.Query.Page, request.Query.Size);

            var foodDtos = _mapper.Map<IEnumerable<FoodDto>>(foods.Data);

            return new PaginatedList<FoodDto>(foods.Page, foods.Size, foods.TotalElements, foodDtos);
        }

        private static IQueryable<Food> FilterByNumber(IQueryable<Food> query, string filterValue, string filterBy)
        {
            if (filterValue.StartsWith('>'))
            {
                if (filterValue[1..] is { Length: > 0 } substring && double.TryParse(substring, out var parsedValue))
                {
                    query = filterBy.ToLower() switch
                    {
                        "price" => query.Where(f => f.Price > parsedValue),
                        "quantity" => query.Where(f => f.Quantity > parsedValue),
                        _ => query
                    };
                }
            }
            else if (filterValue.StartsWith('<'))
            {
                if (filterValue[1..] is { Length: > 0 } substring && double.TryParse(substring, out var parsedValue))
                {
                    query = filterBy.ToLower() switch
                    {
                        "price" => query.Where(f => f.Price < parsedValue),
                        "quantity" => query.Where(f => f.Quantity < parsedValue),
                        _ => query
                    };
                }
            }
            else if (filterValue.StartsWith('='))
            {
                if (filterValue[1..] is { Length: > 0 } substring && double.TryParse(substring, out var parsedValue))
                {
                    query = filterBy.ToLower() switch
                    {
                        "price" => query.Where(f => f.Price.Equals(parsedValue)),
                        "quantity" => query.Where(f => f.Quantity.Equals(parsedValue)),
                        _ => query
                    };
                }
            }
            else if (filterValue.Contains('-'))
            {
                var parts = filterValue.Split('-');
                if (parts.Length == 2
                    && double.TryParse(parts[0], out var minValue)
                    && double.TryParse(parts[1], out var maxValue))
                {
                    query = filterBy.ToLower() switch
                    {
                        "price" => query.Where(f => f.Price >= minValue && f.Price <= maxValue),
                        "quantity" => query.Where(f => f.Quantity >= minValue && f.Quantity <= maxValue),
                        _ => query
                    };
                }
            }
            else
            {
                if (double.TryParse(filterValue, out var parsedValue))
                {
                    query = filterBy.ToLower() switch
                    {
                        "price" => query.Where(f => f.Price.Equals(parsedValue)),
                        "quantity" => query.Where(f => f.Quantity.Equals(parsedValue)),
                        _ => query
                    };
                }
            }

            return query;
        }
        
        private static IQueryable<Food> ApplySorting(IQueryable<Food> query, string sortBy, string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    return sortOrder.ToLower().Equals("asc")
                        ? query.OrderBy(f => f.Name)
                        : query.OrderByDescending(f => f.Name);
                case "price":
                    return sortOrder.ToLower().Equals("asc")
                        ? query.OrderBy(f => f.Price)
                        : query.OrderByDescending(f => f.Price);
                case "quantity":
                    return sortOrder.ToLower().Equals("asc")
                        ? query.OrderBy(f => f.Quantity)
                        : query.OrderByDescending(f => f.Quantity);
                default:
                    return query.OrderByDescending(f => f.CreateAt);
            }
        }
    }
}