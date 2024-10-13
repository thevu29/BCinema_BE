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
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IMediator mediator, ILogger<RoleController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleCommand command)
        {
            try
            {
                var role = await _mediator.Send(command);
                return Ok(new ApiResponse<RoleDto>(true, "Role created successfully", role));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating role.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var roles = await _mediator.Send(new GetRolesQuery());
                return Ok(new ApiResponse<IEnumerable<RoleDto>>(true, "Get all roles successfully", roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all roles.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            try
            {
                var role = await _mediator.Send(new GetRoleByIdQuery { Id = id });
                return Ok(new ApiResponse<RoleDto>(true, "Get role successfully", role));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting role.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}
