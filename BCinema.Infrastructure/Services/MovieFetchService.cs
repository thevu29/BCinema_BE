using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using BCinema.Application.Exceptions;

namespace BCinema.Infrastructure.Services;

public class MovieFetchService(IConfiguration configuration) : IMovieFetchService
{
    private readonly RestClient _client = new("https://api.themoviedb.org/3");
    private readonly string _apiKey = configuration["ApiSettings:TheMovieDbApiKey"]
                                      ?? throw new Exception("TheMovieDbApiKey is missing in appsettings.json");

    public async Task<dynamic?> FetchNowPlayingMoviesAsync(int page)
    {
        var request = new RestRequest($"movie/now_playing?language=vi&page={page}&region=vn");
        request.AddParameter("api_key", _apiKey);
        
        var response = await _client.ExecuteAsync(request);
        
        if (response is { IsSuccessful: false, Content: not null })
        {
            using var document = JsonDocument.Parse(response.Content);
            throw new BadRequestException(document.RootElement.GetProperty("status_message").GetString() ?? "An error occured");
        }

        var movieResponse = response.Content != null
            ? JsonConvert.DeserializeObject<MoviesDto>(response.Content)
            : null;
            
        foreach (var movie in movieResponse?.Results ?? [])
        {
            var movieDetail = await FetchMovieByIdAsync(movie.Id);
            movie.Runtime = movieDetail?.Runtime;
            movie.Genres = movieDetail?.Genres ?? "";
        }

        return movieResponse;
    }

    public async Task<dynamic?> FetchUpcomingMoviesAsync(int page)
    {
        var request = new RestRequest($"movie/upcoming?language=vi&page={page}&region=vn");
        request.AddParameter("api_key", _apiKey);
        
        var response = await _client.ExecuteAsync(request);
        
        if (response is { IsSuccessful: false, Content: not null })
        {
            using var document = JsonDocument.Parse(response.Content);
            throw new BadRequestException(document.RootElement.GetProperty("status_message").GetString() ?? "An error occured");
        }

        var movieResponse = response.Content != null
            ? JsonConvert.DeserializeObject<MoviesDto>(response.Content)
            : null;
            
        foreach (var movie in movieResponse?.Results ?? Enumerable.Empty<MovieDto>())
        {
            var movieDetail = await FetchMovieByIdAsync(movie.Id);
            movie.Runtime = movieDetail?.Runtime;
            movie.Genres = movieDetail?.Genres ?? "";
        }

        return movieResponse;
    }
    
    public async Task<dynamic?> FetchMovieByIdAsync(int id)
    {
        var request = new RestRequest($"movie/{id}?language=vi");
        request.AddParameter("api_key", _apiKey);
        
        var response = await _client.ExecuteAsync(request);

        if (response is { IsSuccessful: false, Content: not null })
        {
            using var document = JsonDocument.Parse(response.Content);
            throw new BadRequestException(document.RootElement.GetProperty("status_message").GetString() ?? "An error occured");
        }
        
        var movieDetailDto = response.Content != null
            ? JsonConvert.DeserializeObject<MovieDetailDto>(response.Content)
            : null;
            
        return movieDetailDto;
    }
    
    public async Task<dynamic?> FetchSearchMovieByAsync(string query, int page)
    {
        var request = new RestRequest($"search/movie?language=vi&region=vn&query={query}&page={page}");
        request.AddParameter("api_key", _apiKey);
        
        var response = await _client.ExecuteAsync(request);

        if (response is { IsSuccessful: false, Content: not null })
        {
            using var document = JsonDocument.Parse(response.Content);
            throw new BadRequestException(document.RootElement.GetProperty("status_message").GetString() ?? "An error occured");
        }
        
        var movieResponse = response.Content != null
            ? JsonConvert.DeserializeObject<MoviesDto>(response.Content)
            : null;
            
        foreach (var movie in movieResponse?.Results ?? [])
        {
            var movieDetail = await FetchMovieByIdAsync(movie.Id);
            movie.Runtime = movieDetail?.Runtime;
            movie.Genres = movieDetail?.Genres ?? "";
        }

        return movieResponse;
    }
}