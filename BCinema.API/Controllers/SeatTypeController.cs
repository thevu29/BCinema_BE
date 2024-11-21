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
    public class SeatTypeController(IMediator mediator, ILogger<SeatTypeController> logger) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSeatTypes()
        {
            try
            {
                var seatTypes = await mediator.Send(new GetAllSeatTypesQuery());
                return Ok(new ApiResponse<IEnumerable<SeatTypeDto>>(true, "Get all seat types successfully", seatTypes));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting rooms");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSeatTypes([FromQuery] SeatTypeQuery query)
        {
            try
            {
                var seatTypes = await mediator.Send(new GetSeatTypesQuery { Query = query });
                return Ok(new PageResponse<IEnumerable<SeatTypeDto>>(
                    true,
                    "Get seat types successfully",
                    seatTypes.Data,
                    seatTypes.Page,
                    seatTypes.Size,
                    seatTypes.TotalPages,
                    seatTypes.TotalElements));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while retrieving seat types");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSeatTypeById(Guid id)
        {
            try
            {
                var seatType = await mediator.Send(new GetSeatTypeById { Id = id });

                return Ok(new ApiResponse<SeatTypeDto>(true, "Get seat type successfully", seatType));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while retrieving seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeatType([FromBody] CreateSeatTypeCommand command)
        {
            try
            {
                var seatType = await mediator.Send(command);
                return Ok(new ApiResponse<SeatTypeDto>(true, "Seat type created successfully", seatType));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSeatType(Guid id, [FromBody] UpdateSeatTypeCommand command)
        {
            try
            {
                command.Id = id;
                var seatType = await mediator.Send(command);

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
                logger.LogError(ex, "An unexpected error occurred while updating seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSeatType(Guid id)
        {
            try
            {
                await mediator.Send(new DeleteSeatTypeCommand { Id = id });
                return Ok(new ApiResponse<string>(true, "Seat type deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while deleting seat type");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}
