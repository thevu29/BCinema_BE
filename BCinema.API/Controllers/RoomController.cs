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
    public class RoomController(IMediator mediator, ILogger<RoomController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRooms([FromQuery] RoomQuery query)
        {
            try
            {
                var rooms = await mediator.Send(new GetRoomsQuery { Query = query });

                return Ok(new PageResponse<IEnumerable<RoomDto>>(
                    true,
                    "Rooms retrieved successfully",
                    rooms.Data,
                    rooms.Page,
                    rooms.Size,
                    rooms.TotalPages,
                    rooms.TotalElements));
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
                logger.LogError(ex, "Error getting rooms");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            try
            {
                var room = await mediator.Send(new GetRoomByIdQuery { Id = id });
                return Ok(new ApiResponse<RoomDto>(true, "Room retrieved successfully", room));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            try
            {
                var room = await mediator.Send(command);
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
                logger.LogError(ex, "Error creating room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
        {
            try
            {
                command.Id = id;
                var room = await mediator.Send(command);
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
                logger.LogError(ex, "Error updating room");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}
