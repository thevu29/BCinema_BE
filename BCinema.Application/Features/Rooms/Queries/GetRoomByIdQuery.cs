using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Rooms.Queries;

public class GetRoomByIdQuery : IRequest<RoomDto>
{
    public Guid Id { get; set; }

    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        
        public GetRoomByIdQueryHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        
        public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.Id, cancellationToken) 
                       ?? throw new NotFoundException(nameof(Room));
            
            return _mapper.Map<RoomDto>(room);
        }
    }
}