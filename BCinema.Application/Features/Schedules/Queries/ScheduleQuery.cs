using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Schedules.Queries;

public class ScheduleQuery : PaginationQuery
{
    public string? Search { get; set; }
    public string? Date { get; set; }
    public int? MovieId { get; set; }
    public Guid? RoomId { get; set; }
    public string? Status { get; set; }
    
    public ScheduleQuery() : base() {}

    public ScheduleQuery(int page, int size, string? search, string? date, int? movieId, Guid? roomId, string? status) 
        : base(page, size)
    {
        Search = search;
        Date = date;
        MovieId = movieId;
        RoomId = roomId;
        Status = status;
    }
}