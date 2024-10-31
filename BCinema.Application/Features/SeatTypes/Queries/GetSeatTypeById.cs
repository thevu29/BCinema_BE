using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class GetSeatTypeById : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }

        public class GetSeatTypeByIdHandler : IRequestHandler<GetSeatTypeById, SeatTypeDto>
        {
            private readonly ISeatTypeRepository _seatTypeRepository;
            private readonly IMapper _mapper;

            public GetSeatTypeByIdHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            {
                _seatTypeRepository = seatTypeRepository;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(GetSeatTypeById request, CancellationToken cancellationToken)
            {
                var seatType = await _seatTypeRepository.GetByIdAsync(request.Id, cancellationToken);

                if (seatType == null)
                {
                    throw new NotFoundException(nameof(SeatType));
                }

                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
