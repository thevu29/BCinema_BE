using BCinema.Application.DTOs;
using BCinema.Domain.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using BCinema.Application.Exceptions;

namespace BCinema.Infrastructure.Services;

public class MovieFetchService : IMovieFetchService
{
    private readonly RestClient _client;
    private readonly string _apiKey;
    
    public MovieFetchService(IConfiguration configuration)
    {
        _client = new RestClient("https://api.themoviedb.org/3");
        _apiKey = configuration["ApiSettings:TheMovieDbApiKey"]
                  ?? throw new Exception("TheMovieDbApiKey is missing in appsettings.json");
    }
    
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
            
        foreach (var movie in movieResponse?.Results ?? Enumerable.Empty<MovieDto>())
        {
            var movieDetail = await FetchMovieByIdAsync(movie.Id);
            movie.Runtime = movieDetail?.Runtime;
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
}