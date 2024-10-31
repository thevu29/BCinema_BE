using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Application.Interfaces;
using BCinema.Domain.Entities;
using MediatR;


namespace BCinema.Application.Features.Foods.Queries
{
    public class GetFoodsQuery : IRequest<PaginatedList<FoodDto>>
    {
        public FoodQuery Query { get; set; } = default!;

        public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, PaginatedList<FoodDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetFoodsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PaginatedList<FoodDto>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
            {
                IQueryable<Food> query = _context.Foods;
                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var foods = await PaginatedList<Food>
                    .ToPageList(query, request.Query.Page, request.Query.Size);
                var foodDtos = _mapper.Map<IEnumerable<FoodDto>>(foods.Data);
                return new PaginatedList<FoodDto>(foods.Page, foods.Size, foods.TotalElements, foodDtos);
            }

            public static IQueryable<Food> ApplySorting(IQueryable<Food> query, string sortBy, string sortOrder)
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(f => f.Name)
                            : query.OrderByDescending(f => f.Name);
                        break;
                    case "price":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(f => f.Price)
                            : query.OrderByDescending(f => f.Price);
                        break;
                    case "createdat":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(f => f.CreateAt)
                            : query.OrderByDescending(f => f.CreateAt);
                        break;
                    default:
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(f => f.Id)
                            : query.OrderByDescending(f => f.Id);
                        break;
                }

                return query;
            }
        }
    }
}
