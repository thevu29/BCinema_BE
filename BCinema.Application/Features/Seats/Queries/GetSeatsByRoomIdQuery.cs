using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Seats.Queries;

public class GetSeatsByRoomIdQuery : IRequest<IEnumerable<SeatDto>>
{
    public Guid RoomId { get; set; }

    public class GetSeatsByRoomIdQueryHandler : IRequestHandler<GetSeatsByRoomIdQuery, IEnumerable<SeatDto>>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        
        public GetSeatsByRoomIdQueryHandler(
            ISeatRepository seatRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<SeatDto>> Handle(GetSeatsByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId, cancellationToken)
                ?? throw new NotFoundException("Room not found");
            
            var seats = await _seatRepository.GetSeatsByRoomIdAsync(room.Id, cancellationToken);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }
    }
}