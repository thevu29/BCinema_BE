﻿using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Schedules.Queries;

public class ScheduleQuery : PaginationQuery
{
    public string? Date { get; set; }
    public int? MovieId { get; set; }
    public Guid? RoomId { get; set; }
    public string? Status { get; set; }
    
    public ScheduleQuery() : base() {}

    public ScheduleQuery(int page, int size, string? date, int? movieId, Guid? roomId, string? status) 
        : base(page, size)
    {
        this.Date = date;
        this.MovieId = movieId;
        this.RoomId = roomId;
        this.Status = status;
    }
}