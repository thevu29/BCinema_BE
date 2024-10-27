using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Application.Interfaces;
using BCinema.Doman.Entities;
using MediatR;

namespace BCinema.Application.Features.Seats.Queries
{
    public class GetSeatsQuery : IRequest<PaginatedList<SeatDto>>
    {
        public SeatQuery Query { get; set; } = default!;

        public class GetSeatsQueryHandler : IRequestHandler<GetSeatsQuery, PaginatedList<SeatDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetSeatsQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PaginatedList<SeatDto>> Handle(GetSeatsQuery request, CancellationToken cancellationToken)
            {
                IQueryable<Seat> query = _context.Seats;
                if (!string.IsNullOrEmpty(request.Query.Row))
                {
                    query = query.Where(s => s.Row.ToLower().Contains(request.Query.Row.ToLower()));
                }
                if (request.Query.Number.HasValue)
                {
                    query = query.Where(s => s.Number == request.Query.Number.Value);
                }
                if (request.Query.RoomId.HasValue)
                {
                    query = query.Where(s => s.RoomId == request.Query.RoomId.Value);
                }
                if (request.Query.Status.HasValue)
                {
                    query = query.Where(s => s.Status == request.Query.Status.Value);
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                query = query.Where(st => st.UpdateAt == null);

                var paginatedSeats = await PaginatedList<Seat>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var seatDtos = _mapper.Map<IEnumerable<SeatDto>>(paginatedSeats.Data);
                return new PaginatedList<SeatDto>(
                    paginatedSeats.Page,
                    paginatedSeats.Size,
                    paginatedSeats.TotalElements,
                    seatDtos
                );
            }
            public static IQueryable<Seat> ApplySorting(IQueryable<Seat> query, string sortBy, string sortOrder)
            {
                switch (sortBy.ToLower())
                {
                    case "row":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Row)
                            : query.OrderByDescending(s => s.Row);
                        break;
                    case "number":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Number)
                            : query.OrderByDescending(s => s.Number);
                        break;
                    case "roomid":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.RoomId)
                            : query.OrderByDescending(s => s.RoomId);
                        break;
                    case "status":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Status)
                            : query.OrderByDescending(s => s.Status);
                        break;
                    default:
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Id)
                            : query.OrderByDescending(s => s.Id);
                        break;
                }

                return query;
            }
        }
    }
}
