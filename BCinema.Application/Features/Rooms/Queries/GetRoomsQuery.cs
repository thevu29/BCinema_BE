using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Rooms.Queries;

public class GetRoomsQuery : IRequest<PaginatedList<RoomDto>>
{
    public RoomQuery Query { get; set; } = default!;

    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, PaginatedList<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        
        public GetRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
        {
            var query = _roomRepository.GetRooms();
            
            if (!string.IsNullOrWhiteSpace(request.Query.Name))
            {
                query = query.Where(r => r.Name.Contains(request.Query.Name));
            }
            
            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var rooms = await PaginatedList<Room>
                .ToPageList(query, request.Query.Page, request.Query.Size);
            
            var roomsDto = _mapper.Map<IEnumerable<RoomDto>>(rooms.Data);
            
            return new PaginatedList<RoomDto>(
                rooms.Page,
                rooms.Size,
                rooms.TotalElements,
                roomsDto);
        }
        
        private static IQueryable<Room> ApplySorting(
            IQueryable<Room> query,
            string sortBy,
            string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    return sortOrder.ToUpper().Equals("ASC") 
                        ? query.OrderBy(r => r.Name)
                        : query.OrderByDescending(r => r.Name);
                default:
                    return sortOrder.ToUpper().Equals("ASC") 
                        ? query.OrderBy(r => r.Id)
                        : query.OrderByDescending(r => r.Id);
            }
        }
    }
}