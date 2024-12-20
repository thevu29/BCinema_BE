﻿using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Users.Commands;
using BCinema.API.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BCinema.Application.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;

namespace BCinema.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController(IMediator mediator, ILogger<UserController> logger) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await mediator.Send(new GetAllUsersQuery());
                return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Get all users successfully", users));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting all users");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await mediator.Send(new DeleteUserCommand { Id = id });

                return Ok(new ApiResponse<string>(true, "User deleted successfully"));
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
                logger.LogError(ex, "An unexpected error occurred while deleting user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserCommand command)
        {
            try
            {
                command.Id = id;
                var user = await mediator.Send(command);

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
                logger.LogError(ex, "An unexpected error occurred while updating user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserCommand command)
        {
            try
            {
                var user = await mediator.Send(command);
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
                logger.LogError(ex, "An unexpected error occurred while creating user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await mediator.Send(new GetUserByEmailQuery { Email = email });

                return Ok(new ApiResponse<UserDto>(true, "Get user successfully", user));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting user by id");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await mediator.Send(new GetUserByIdQuery { Id = id });

                return Ok(new ApiResponse<UserDto>(true, "Get user successfully", user));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting user by id");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserQuery query)
        {
            try
            {
                var users = await mediator.Send(new GetUsersQuery { Query = query });

                return Ok(new PageResponse<IEnumerable<UserDto>>(
                    true, "Get users successfully",
                    users.Data,
                    users.Page,
                    users.Size,
                    users.TotalPages,
                    users.TotalElements));
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
                logger.LogError(ex, "An unexpected error occurred while getting users");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpGet("registration")]
        public async Task<IActionResult> GetCountUser([FromQuery] int year, int month)
        {
            try
            {
                var count = await mediator.Send(new GetCountUserQuery { Year = year, Month = month });

                return Ok(new ApiResponse<int>(true, "Get count user successfully", count));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting count user");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}