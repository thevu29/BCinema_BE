using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Schedules.Commands;
using BCinema.Application.Features.Schedules.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/schedules")]
[ApiController]
public class ScheduleController(IMediator mediator, ILogger<ScheduleController> logger) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSchedules()
    {
        try
        {
            var schedules = await mediator.Send(new GetAllSchedulesQuery());
            return Ok(new ApiResponse<IEnumerable<ScheduleDto>>(true, "Get all schedules successfully", schedules));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rooms");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSchedules([FromQuery] ScheduleQuery query)
    {
        try
        {
            var schedules = await mediator.Send(new GetSchedulesQuery { Query = query });

            return Ok(new PageResponse<IEnumerable<SchedulesDto>>(
                true,
                "Get schedules successfully",
                schedules.Data, 
                schedules.Page,
                schedules.Size,
                schedules.TotalPages,
                schedules.TotalElements));
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting schedules");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetScheduleById(Guid id)
    {
        try
        {
            var schedule = await mediator.Send(new GetScheduleByIdQuery { Id = id });

            return Ok(new ApiResponse<ScheduleDto>(true, "Get schedule successfully", schedule));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting schedule by id");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] UpdateScheduleCommand command)
    {
        try
        {
            command.Id = id;
            var schedule = await mediator.Send(command);

            return Ok(new ApiResponse<ScheduleDto>(true, "Update schedule successfully", schedule));
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
            logger.LogError(ex, "Error while updating schedule");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSchedules([FromBody] CreateSchedulesCommand command)
    {
        try
        {
            var schedule = await mediator.Send(command);
            
            return StatusCode(
                StatusCodes.Status201Created, 
                new ApiResponse<SchedulesDto>(true, "Create schedules successfully", schedule));
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
            logger.LogError(ex, "Error while creating schedules");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
}