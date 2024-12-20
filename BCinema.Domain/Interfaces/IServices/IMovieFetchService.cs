﻿namespace BCinema.Domain.Interfaces.IServices;

public interface IMovieFetchService
{
    Task<dynamic?> FetchNowPlayingMoviesAsync(int page);
    Task<dynamic?> FetchUpcomingMoviesAsync(int page);
    Task<dynamic?> FetchMovieByIdAsync(int id);
    Task<dynamic?> FetchSearchMovieByAsync(string query, int page);
}