using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Domain.Entities;
using MediatR;

namespace BCinema.Application.Features.SeatTypes.Commands
{
    public class UpdateSeatTypeCommand : IRequest<SeatTypeDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }

        public class UpdateSeatTypeCommandHandler : IRequestHandler<UpdateSeatTypeCommand, SeatTypeDto>
        {
            private readonly ISeatTypeRepository _seatTypeRepository;
            private readonly IMapper _mapper;

            public UpdateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IMapper mapper)
            {
                _seatTypeRepository = seatTypeRepository;
                _mapper = mapper;
            }

            public async Task<SeatTypeDto> Handle(
                UpdateSeatTypeCommand request,
                CancellationToken cancellationToken)
            {
                var seatType = await _seatTypeRepository
                    .GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(SeatType));
               
                _mapper.Map(request, seatType);

                await _seatTypeRepository.SaveChangesAsync(cancellationToken);

                return _mapper.Map<SeatTypeDto>(seatType);
            }
        }
    }
}
