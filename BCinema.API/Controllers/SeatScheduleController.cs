using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.SeatSchedules.Commands;
using BCinema.Application.Features.SeatSchedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/seat-schedules")]
[ApiController]
public class SeatScheduleController(IMediator mediator, ILogger<SeatScheduleController> logger) : ControllerBase
{
    [HttpDelete("schedule/{scheduleId:guid}")]
    public async Task<IActionResult> DeleteSeatsInSchedule(Guid scheduleId)
    {
        try
        {
            await mediator.Send(new DeleteSeatsInScheduleCommand() { ScheduleId = scheduleId });
            return Ok(new ApiResponse<string>(true, "Delete seats in schedule successfully"));
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
            logger.LogError(ex, "An error occurred while deleting seats in schedule");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSeatScheduleById(Guid id)
    {
        try
        {
            var seatSchedule = await mediator.Send(new GetSeatScheduleByIdQuery() { Id = id });
            return Ok(new ApiResponse<SeatScheduleDto>(true, "Get seat schedule successfully", seatSchedule));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting seat schedule");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpGet("schedule/{scheduleId:guid}/seat/{seatId:guid}")]
    public async Task<IActionResult> GetSeatScheduleBySeatIdAndScheduleId(Guid scheduleId, Guid seatId)
    {
        try
        {
            var seatSchedule = await mediator.Send(new GetSeatScheduleBySdIdAndSId() { ScheduleId = scheduleId, SeatId = seatId });
            return Ok(new ApiResponse<SeatScheduleDto>(true, "Get seat schedule successfully", seatSchedule));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting seat schedule");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpGet("schedule/{scheduleId:guid}")]
    public async Task<IActionResult> GetSeatSchedulesByScheduleId(Guid scheduleId)
    {
        try
        {
            var seatSchedules = await mediator.Send(new GetSeatSchedulesBySIdQuery() { ScheduleId = scheduleId });
            return Ok(new ApiResponse<IEnumerable<SeatScheduleDto>>(true, "Get seats successfully" , seatSchedules));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting seat schedules");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
}