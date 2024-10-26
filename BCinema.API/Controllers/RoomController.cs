using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Features.Rooms.Queries;
using BCinema.Application.Features.Schedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoomController> _logger;

        public RoomController(IMediator mediator, ILogger<RoomController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRoom()
        {
            try
            {
                var rooms = await _mediator.Send(new GetAllRoomQuery());
                return Ok(new ApiResponse<IEnumerable<RoomDto>>(true, "Get all rooms successfully", rooms));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all rooms.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomById(Guid id)
        {
            try
            {
                var room = await _mediator.Send(new GetRoomByIdQuery { Id = id });
                return Ok(new ApiResponse<RoomDto>(true, "Get room by id successfully", room));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting room by id.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{roomId}/schedules")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetScheduleByRoomId(Guid roomId)
        {
            try
            {
                var schedules = await _mediator.Send(new GetSchedulesByRoomIdQuery { RoomId = roomId });
                return Ok(new ApiResponse<IEnumerable<ScheduleDto>>(true, "Schedules fetched successfully", schedules));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching schedules.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }


        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomCommand command)
        {
            try
            {
                var room = await _mediator.Send(command);
                return Ok(new ApiResponse<RoomDto>(true, "Room created successfully", room));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating room.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> UpdateRoom(Guid id, UpdateRoomCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new ApiResponse<string>(false, "ID mismatch"));
            }

            try
            {
                var room = await _mediator.Send(command);
                return Ok(new ApiResponse<RoomDto>(true, "Room updated successfully", room));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating room.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RoomDto>> DeleteRoom(Guid id)
        {
            try
            {
                var room = await _mediator.Send(new DeleteRoomCommand { Id = id });
                    return Ok(new ApiResponse<RoomDto>(true, "Room deleted successfully"));
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
                _logger.LogError(ex, "An unexpected error occurred while deleting room.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}
