using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Schedules.Commands;
using BCinema.Application.Features.Schedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;


namespace BCinema.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAllSchedule()
        {
            try
            {
                var schedules = await _mediator.Send(new GetAllSchedulesQuery());
                return Ok(new ApiResponse<IEnumerable<ScheduleDto>>(true, "Get all schedules successfully", schedules));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all schedules.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetScheduleById(Guid id)
        {
            try
            {
                var schedule = await _mediator.Send(new GetScheduleByIdQuery { Id = id });
                return Ok(new ApiResponse<ScheduleDto>(true, "Get schedule successfully", schedule));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all schedules.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{movieId}/schedules")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedulesByMovieId(string movieId)
        {
            try
            {
                var schedules = await _mediator.Send(new GetSchedulesByMovieIdQuery { MovieId = movieId });
                return Ok(new ApiResponse<IEnumerable<ScheduleDto>>(true, "Get schedules by movie id successfully", schedules));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting schedules by movie id.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule([FromBody] CreateScheduleCommand command)
        {
            try
            {
                var schedule = await _mediator.Send(command);
                return Ok(new ApiResponse<ScheduleDto>(true, "Schedule created successfully", schedule));
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
                _logger.LogError(ex, "An unexpected error occurred while creating schedule.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduleDto>> UpdateSchedule(Guid id, [FromBody] UpdateScheduleCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new ApiResponse<string>(false, "ID mismatch"));
            }

            try
            {
                var schedule = await _mediator.Send(command);
                return Ok(new ApiResponse<ScheduleDto>(true, "Schedule updated successfully", schedule));
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
                _logger.LogError(ex, "An unexpected error occurred while updating schedule.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ScheduleDto>> DeleteSchedule(Guid id)
        {
            try
            {
                var schedule = await _mediator.Send(new DeleteScheduleCommand { Id = id });
                return Ok(new ApiResponse<ScheduleDto>(true, "Schedule deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting schedule.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}
