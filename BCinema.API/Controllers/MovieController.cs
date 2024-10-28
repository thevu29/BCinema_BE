using BCinema.API.Responses;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieFetchService _movieFetchService;
    private readonly ILogger<MovieController> _logger;
    
    public MovieController(IMovieFetchService movieFetchService, ILogger<MovieController> logger)
    {
        _movieFetchService = movieFetchService;
        _logger = logger;
    }
    
    [HttpGet("upcoming/{page}")]
    public async Task<IActionResult> GetUpcomingMoviesAsync(int page)
    {
        try
        {
            var movies = await _movieFetchService.FetchUpcomingMoviesAsync(page);

            return Ok(new ApiResponse<dynamic>(true, "Get upcoming movies successfully", movies));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch upcoming movies from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpGet("now-playing/{page}")]
    public async Task<IActionResult> GetNowPlayingMoviesAsync(int page)
    {
        try
        {
            var movies = await _movieFetchService.FetchNowPlayingMoviesAsync(page);

            return Ok(new ApiResponse<dynamic>(true, "Get now playing movies successfully", movies));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch now playing movies from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieByIdAsync(int id)
    {
        try
        {
            var movie = await _movieFetchService.FetchMovieByIdAsync(id);
            
            return Ok(new ApiResponse<dynamic>(true, "Get movie successfully", movie));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch movie from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
}