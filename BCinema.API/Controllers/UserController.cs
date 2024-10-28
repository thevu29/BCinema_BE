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
    [Route("api/users")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand { Id = id });

                return Ok(new ApiResponse<string>(true, "User deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserCommand command)
        {
            try
            {
                command.Id = id;
                var user = await _mediator.Send(command);

                return Ok(new ApiResponse<UserDto>(true, "User updated successfully", user));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserCommand command)
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
                _logger.LogError(ex, "An unexpected error occurred while creating user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByEmailQuery { Email = email });

                return Ok(new ApiResponse<UserDto>(true, "Get user successfully", user));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting user by id");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery { Id = id });

                return Ok(new ApiResponse<UserDto>(true, "Get user successfully", user));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting user by id");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            try
            {
                var users = await _mediator.Send(new GetUsersQuery { Query = query });

                return Ok(new PageResponse<IEnumerable<UserDto>>(
                    true, "Get users successfully",
                    users.Data,
                    users.Page,
                    users.Size,
                    users.TotalPages,
                    users.TotalElements));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting users");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}