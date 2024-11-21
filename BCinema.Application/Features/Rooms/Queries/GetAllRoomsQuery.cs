using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Rooms.Queries;

public class GetAllRoomsQuery : IRequest<IEnumerable<RoomDto>>
{
    public class GetAllRoomsQueryHandler(
        IRoomRepository roomRepository,
        IMapper mapper) : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDto>>
    {
        public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await roomRepository.GetRoomsAsync(cancellationToken);
            return mapper.Map<IEnumerable<RoomDto>>(rooms);
        }
    }
}