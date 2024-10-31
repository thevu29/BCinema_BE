using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.SeatTypes.Commands;
using BCinema.Application.Features.SeatTypes.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers
{
    [Route("api/seat-types")]
    [ApiController]
    public class SeatTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeatTypeController> _logger;

        public SeatTypeController(IMediator mediator, ILogger<SeatTypeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSeatTypes([FromQuery] SeatTypeQuery query)
        {
            try
            {
                var seatTypes = await _mediator.Send(new GetSeatTypesQuery { Query = query });
                return Ok(new PageResponse<IEnumerable<SeatTypeDto>>(
                    true,
                    "Get seat types successfully",
                    seatTypes.Data,
                    seatTypes.Page,
                    seatTypes.Size,
                    seatTypes.TotalPages,
                    seatTypes.TotalElements));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving seat types");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatTypeById(Guid id)
        {
            try
            {
                var seatType = await _mediator.Send(new GetSeatTypeById { Id = id });

                return Ok(new ApiResponse<SeatTypeDto>(true, "Get seat type successfully", seatType));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeatType([FromBody] CreateSeatTypeCommand command)
        {
            try
            {
                var seatType = await _mediator.Send(command);
                return Ok(new ApiResponse<SeatTypeDto>(true, "Seat type created successfully", seatType));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatType(Guid id, [FromBody] UpdateSeatTypeCommand command)
        {
            try
            {
                command.Id = id;
                var seatType = await _mediator.Send(command);

                return Ok(new ApiResponse<SeatTypeDto>(true, "Seat type updated successfully", seatType));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}
