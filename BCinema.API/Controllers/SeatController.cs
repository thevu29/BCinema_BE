using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Features.Seats.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BCinema.API.Controllers
{
    [Route("api/seats")]
    [ApiController]
    public class SeatController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeatController> _logger;

        public SeatController(IMediator mediator, ILogger<SeatController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeats([FromQuery] SeatQuery query)
        {
            try
            {
                var seats = await _mediator.Send(new GetSeatsQuery { Query = query });
                return Ok(new PageResponse<IEnumerable<SeatDto>>(true, "Get all seats successfully",
                    seats.Data, seats.Page, seats.Size, seats.TotalPages, seats.TotalElements));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all seats.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));

            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(Guid id)
        {
            try
            {
                var seat = await _mediator.Send(new GetSeatByIdQuery { Id = id });
                return Ok(new ApiResponse<SeatDto>(true, "Get seat by id successfully", seat));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting seat by id.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeat([FromBody] CreateSeatCommand command)
        {
            try
            {
                var seat = await _mediator.Send(command);
                return Ok(new ApiResponse<SeatDto>(true, "Seat created successfully", seat));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating seat.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] UpdateSeatCommand command)
        {
            try
            {
                command.Id = id;
                var seat = await _mediator.Send(command);
                return Ok(new ApiResponse<SeatDto>(true, "Seat updated successfully", seat));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating seat.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteSeatCommand { Id = id });
                return Ok(new ApiResponse<string>(true, "Seat deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting seat.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}
