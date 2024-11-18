using BCinema.Application.DTOs;
using BCinema.Application.Features.Roles.Commands;
using BCinema.Application.Features.Roles.Queries;
using BCinema.API.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BCinema.Application.Exceptions;
using FluentValidation;

namespace BCinema.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController(IMediator mediator, ILogger<RoleController> logger) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await mediator.Send(new GetAllRolesQuery());
                return Ok(new ApiResponse<IEnumerable<RoleDto>>(true, "Get all roles successfully", roles));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating role");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, [FromBody] UpdateRoleCommand command)
        {
            try
            {
                command.Id = id;
                var role = await mediator.Send(command);
                return Ok(new ApiResponse<RoleDto>(true, "Role updated successfully", role));
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
                logger.LogError(ex, "An unexpected error occurred while updating role");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleCommand command)
        {
            try
            {
                var role = await mediator.Send(command);
                return Ok(new ApiResponse<RoleDto>(true, "Role created successfully", role));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while creating role");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] RoleQuery query)
        {
            try
            {
                var roles = await mediator.Send(new GetRolesQuery { Query = query });

                return Ok(new PageResponse<IEnumerable<RoleDto>>(
                    true,
                    "Get roles successfully",
                    roles.Data,
                    roles.Page,
                    roles.Size,
                    roles.TotalPages,
                    roles.TotalElements));
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
                logger.LogError(ex, "An unexpected error occurred while getting roles");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            try
            {
                var role = await mediator.Send(new GetRoleByIdQuery { Id = id });
                return Ok(new ApiResponse<RoleDto>(true, "Get role successfully", role));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while getting role");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
            }
        }
    }
}
