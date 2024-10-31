using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypesQuery : IRequest<PaginatedList<SeatTypeDto>>
    {
        public SeatTypeQuery Query { get; set; } = default!;

        public class GetSeatTypesQueryHandler : IRequestHandler<GetSeatTypesQuery, PaginatedList<SeatTypeDto>>
        {
            private readonly ISeatTypeRepository _seatTypeRepository;
            private readonly IMapper _mapper;

            public GetSeatTypesQueryHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            {
                _seatTypeRepository = seatTypeRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedList<SeatTypeDto>> Handle(
                GetSeatTypesQuery request,
                CancellationToken cancellationToken)
            {

                IQueryable<SeatType> query = _seatTypeRepository.GetSeatTypes();

                if (!string.IsNullOrEmpty(request.Query.Name))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Query.Name.ToLower()));
                }

                query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);

                var seatTypes = await PaginatedList<SeatType>
                    .ToPageList(query, request.Query.Page, request.Query.Size);

                var seatTypeDtos = _mapper.Map<IEnumerable<SeatTypeDto>>(seatTypes.Data);

                return new PaginatedList<SeatTypeDto>(
                    seatTypes.Page,
                    seatTypes.Size,
                    seatTypes.TotalElements,
                    seatTypeDtos);
            }

            private static IQueryable<SeatType> ApplySorting(
                IQueryable<SeatType> query,
                string sortBy,
                string sortOrder)
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Name)
                            : query.OrderByDescending(st => st.Name);
                        break;
                    case "price":
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Price)
                            : query.OrderByDescending(st => st.Price);
                        break;
                    default:
                        query = sortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(st => st.Id)
                            : query.OrderByDescending(st => st.Id);
                        break;
                }

                return query;
            }
        }
    }
}
