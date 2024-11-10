using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Rooms.Queries;

public class RoomQuery : PaginationQuery
{
    public string? Search { get; set; }

    public RoomQuery() : base() {}
    
    public RoomQuery(string? search, int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
        Search = search;
    }
}