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
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ScheduleController> _logger;
    
    public ScheduleController(IMediator mediator, ILogger<ScheduleController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetSchedules([FromQuery] ScheduleQuery query)
    {
        try
        {
            var schedules = await _mediator.Send(new GetSchedulesQuery { Query = query });

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
            _logger.LogError(ex, "Error while getting schedules");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] UpdateScheduleCommand command)
    {
        try
        {
            command.Id = id;
            var schedule = await _mediator.Send(command);

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
            _logger.LogError(ex, "Error while updating schedule");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSchedules([FromBody] CreateSchedulesCommand command)
    {
        try
        {
            var schedule = await _mediator.Send(command);
            
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
            _logger.LogError(ex, "Error while creating schedules");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred"));
        }
    }
}