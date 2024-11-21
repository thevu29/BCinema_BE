using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Seats.Commands;
using BCinema.Application.Features.Seats.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BCinema.API.Controllers;

[Route("api/seats")]
[ApiController]
public class SeatController(IMediator mediator, ILogger<SeatController> logger) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSeatById(Guid id)
    {
        try
        {
            var seat = await mediator.Send(new GetSeatByIdQuery() { Id = id });
            return Ok(new ApiResponse<SeatDto>(true, "Seat retrieved successfully", seat));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving seat");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpGet("room/{roomId:guid}")]
    public async Task<IActionResult> GetSeatsByRoomId(Guid roomId)
    {
        try
        {
            var seats = await mediator.Send(new GetSeatsByRoomIdQuery { RoomId = roomId });
            return Ok(new ApiResponse<IEnumerable<SeatDto>>(true, "Seats retrieved successfully", seats));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving seats");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSeat([FromBody] CreateSeatCommand command)
    {
        try
        {
            var seat = await mediator.Send(command);
            return StatusCode(201, new ApiResponse<SeatDto>(true, "Seat created successfully", seat));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating a seat");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] UpdateSeatCommand command)
    {
        try
        {
            command.Id = id;
            var seat = await mediator.Send(command);
            return Ok(new ApiResponse<SeatDto>(true, "Seat updated successfully", seat));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating a seat");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
}
