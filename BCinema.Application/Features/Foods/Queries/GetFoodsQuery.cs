using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Foods.Queries;

public class GetFoodsQuery : IRequest<PaginatedList<FoodDto>>
{
    public FoodQuery Query { get; init; } = default!;

    public class GetFoodsQueryHandler(IFoodRepository foodRepository, IMapper mapper)
        : IRequestHandler<GetFoodsQuery, PaginatedList<FoodDto>>
    {
        public async Task<PaginatedList<FoodDto>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
        {
            var query = foodRepository.GetFoods();

            if (!string.IsNullOrEmpty(request.Query.Search))
            {
                var searchTerm = request.Query.Search.Trim().ToLower();
                query = query.Where(f => EF.Functions.Like(f.Name.ToLower(), $"%{searchTerm}%"));
            }
            if (!string.IsNullOrEmpty(request.Query.Price))
            {
                query = query.FilterByNumber(request.Query.Price, f => f.Price);
            }
            if (!string.IsNullOrEmpty(request.Query.Quantity))
            {
                query = query.FilterByNumber(request.Query.Quantity, f => f.Quantity);
            }

            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

            var foods = await PaginatedList<Food>.ToPageList(query, request.Query.Page, request.Query.Size);

            var foodDtos = mapper.Map<IEnumerable<FoodDto>>(foods.Data);

            return new PaginatedList<FoodDto>(foods.Page, foods.Size, foods.TotalElements, foodDtos);
        }
        
        private static IQueryable<Food> ApplySorting(IQueryable<Food> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Food.Name),
                nameof(Food.Price),
                nameof(Food.Quantity),
                nameof(Food.CreateAt)
            };
            
            if (string.IsNullOrWhiteSpace(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(f => f.CreateAt);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);
        }
    }
}