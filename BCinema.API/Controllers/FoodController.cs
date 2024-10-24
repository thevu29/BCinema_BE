using BCinema.API.Responses;
using BCinema.Application.Features.Foods.Queries;
using MediatR;
using BCinema.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Foods.Commands;
using System.ComponentModel.DataAnnotations;

namespace BCinema.API.Controllers
{
    [Route("api/foods")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FoodController> _logger;

        public FoodController(IMediator mediator, ILogger<FoodController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery] FoodQuery query)
        {
            try
            {
                var foods = await _mediator.Send(new GetFoodsQuery { Query = query });
                return Ok(new PageResponse<IEnumerable<FoodDto>>(true, "Get foods successfully",
                    foods.Data, foods.Page, foods.Size, foods.TotalPages, foods.TotalElements));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting all foods.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetFoodByName(String name)
        {
            try
            {
                var food = await _mediator.Send(new GetFoodByNameQuery { Name = name });
                return Ok(new ApiResponse<FoodDto>(true, "Get food successfully", food));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting food.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodCommand command)
        {
            try
            {
                var food = await _mediator.Send(command);
                return Ok(new ApiResponse<FoodDto>(true, "Create food successfully", food));
            }
            catch(ValidationException ex)
            {
                _logger.LogError(ex, "An error occurred while creating food.");
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating food.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(Guid id, [FromBody] UpdateFoodCommand command)
        {
            try
            {
                command.Id = id;
                var food = await _mediator.Send(command);
                return Ok(new ApiResponse<FoodDto>(true, "Update food successfully", food));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "An error occurred while updating food.");
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating food.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteFoodCommand { Id = id });
                return Ok(new ApiResponse<string>(true, "Delete food successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(false, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting food.");
                return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
            }
        }
    }
}
