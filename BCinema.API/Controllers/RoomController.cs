using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Features.Rooms.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers
{
    [Route("api/rooms")]
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

        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] RoomQuery query)
        {
            try
            {
                var rooms = await _mediator.Send(new GetRoomsQuery { Query = query });

                return Ok(new PageResponse<IEnumerable<RoomDto>>(
                    true,
                    "Rooms retrieved successfully",
                    rooms.Data,
                    rooms.Page,
                    rooms.Size,
                    rooms.TotalPages,
                    rooms.TotalElements));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rooms");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            try
            {
                var room = await _mediator.Send(new GetRoomByIdQuery { Id = id });
                return Ok(new ApiResponse<RoomDto>(true, "Room retrieved successfully", room));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            try
            {
                var room = await _mediator.Send(command);
                return Ok(new ApiResponse<RoomDto>(true, "Room created successfully", room));
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
                _logger.LogError(ex, "Error creating room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
        {
            try
            {
                command.Id = id;
                var room = await _mediator.Send(command);
                return Ok(new ApiResponse<RoomDto>(true, "Room updated successfully", room));
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
                _logger.LogError(ex, "Error updating room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}
