using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypesQuery : IRequest<PaginatedList<SeatTypeDto>>
    {
        public SeatTypeQuery Query { get; init; } = default!;

        public class GetSeatTypesQueryHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            : IRequestHandler<GetSeatTypesQuery, PaginatedList<SeatTypeDto>>
        {
            public async Task<PaginatedList<SeatTypeDto>> Handle(GetSeatTypesQuery request, CancellationToken cancellationToken)
            {
                var query = seatTypeRepository.GetSeatTypes();

                if (!string.IsNullOrEmpty(request.Query.Search))
                {
                    var searchTerm = request.Query.Search.Trim().ToLower();
                    query = query.Where(st => EF.Functions.Like(st.Name.ToLower(), $"%{searchTerm}%"));
                }
                if (!string.IsNullOrEmpty(request.Query.Price))
                {
                    query = query.FilterByNumber(request.Query.Price, st => st.Price);
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var seatTypes = await PaginatedList<SeatType>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var seatTypeDtos = mapper.Map<IEnumerable<SeatTypeDto>>(seatTypes.Data);

                return new PaginatedList<SeatTypeDto>(
                    seatTypes.Page,
                    seatTypes.Size,
                    seatTypes.TotalElements,
                    seatTypeDtos);
            }

            private static IQueryable<SeatType> ApplySorting(IQueryable<SeatType> query, string sortBy, string sortOrder)
            {
                var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    nameof(SeatType.Name),
                    nameof(SeatType.Price),
                    nameof(SeatType.CreateAt)
                };
                
                if (string.IsNullOrEmpty(sortBy) || !allowedSortColumns.Contains(sortBy))
                {
                    query = query.OrderByDescending(st => st.CreateAt);
                }

                return query.ApplyDynamicSorting(sortBy, sortOrder);
            }
        }
    }
}
