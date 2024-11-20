using BCinema.API.Responses;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/movies")]
[ApiController]
public class MovieController(IMovieFetchService movieFetchService, ILogger<MovieController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSearchMovieByAsync([FromQuery] string query, [FromQuery] int page)
    {
        try
        {
            var movies = await movieFetchService.FetchSearchMovieByAsync(query, page);

            return Ok(new ApiResponse<dynamic>(true, "Get search movies successfully", movies));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch search movies from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpGet("upcoming/{page:int}")]
    public async Task<IActionResult> GetUpcomingMoviesAsync(int page)
    {
        try
        {
            var movies = await movieFetchService.FetchUpcomingMoviesAsync(page);

            return Ok(new ApiResponse<dynamic>(true, "Get upcoming movies successfully", movies));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch upcoming movies from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpGet("now-playing/{page:int}")]
    public async Task<IActionResult> GetNowPlayingMoviesAsync(int page)
    {
        try
        {
            var movies = await movieFetchService.FetchNowPlayingMoviesAsync(page);

            return Ok(new ApiResponse<dynamic>(true, "Get now playing movies successfully", movies));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch now playing movies from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovieByIdAsync(int id)
    {
        try
        {
            var movie = await movieFetchService.FetchMovieByIdAsync(id);
            
            return Ok(new ApiResponse<dynamic>(true, "Get movie successfully", movie));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch movie from TheMovieDb API");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
}