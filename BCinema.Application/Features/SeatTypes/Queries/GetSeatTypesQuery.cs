using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Vouchers.Queries;
using BCinema.Application.Helpers;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypesQuery : IRequest<PaginatedList<SeatTypeDto>>
    {
        public SeatTypeQuery Query { get; set; } = default!;

        public class GetSeatTypesQueryHandler : IRequestHandler<GetSeatTypesQuery, PaginatedList<SeatTypeDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSeatTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PaginatedList<SeatTypeDto>> Handle(GetSeatTypesQuery request, CancellationToken cancellationToken)
            {
                IQueryable<SeatType> query = _context.SeatTypes;
                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(st => st.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }
                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                query = query.Where(st => st.UpdateAt == null);

                var seatTypes = await PaginatedList<SeatType>
                    .ToPageList(query, request.Query.Page, request.Query.Size);
                var seatTypeDtos = _mapper.Map<IEnumerable<SeatTypeDto>>(seatTypes.Data);

                return new PaginatedList<SeatTypeDto>(seatTypes.Page, seatTypes.Size, seatTypes.TotalElements, seatTypeDtos);
            }

            private static IQueryable<SeatType> ApplySorting(IQueryable<SeatType> query, string sortBy, string sortOrder)
            {
                switch (sortBy.ToLower())
                {
                    case "createdat":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(v => v.CreateAt)
                            : query.OrderByDescending(v => v.CreateAt);
                        break;
                    default:
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(v => v.Id)
                            : query.OrderByDescending(v => v.Id);
                        break;
                }
                return query;
            }
        }
    }
}