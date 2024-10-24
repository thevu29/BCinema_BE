using System.ComponentModel.DataAnnotations;
using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Features.Rooms.Commands;
using BCinema.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
