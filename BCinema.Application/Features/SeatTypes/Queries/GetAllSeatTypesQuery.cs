using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Queries;

public class GetAllSeatTypesQuery : IRequest<IEnumerable<SeatTypeDto>>
{
    public class GetAllSeatTypesQueryHandler(
        ISeatTypeRepository seatTypeRepository,
        IMapper mapper) : IRequestHandler<GetAllSeatTypesQuery, IEnumerable<SeatTypeDto>>
    {
        public async Task<IEnumerable<SeatTypeDto>> Handle(GetAllSeatTypesQuery request, CancellationToken cancellationToken)
        {
            var seatTypes = await seatTypeRepository.GetSeatTypesAsync(cancellationToken);
            return mapper.Map<IEnumerable<SeatTypeDto>>(seatTypes);
        }
    }
}