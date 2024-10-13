using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Users.Commands;
using BCinema.API.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BCinema.Application.Features.Users.Queries;

namespace BCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var user = await _mediator.Send(command);
                return Ok(new ApiResponse<UserDto>(true, "User created successfully", user));
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
                _logger.LogError(ex, "An unexpected error occurred while creating user.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _mediator.Send(new GetUsersQuery());
                return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Get all users successfully", users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all users.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}