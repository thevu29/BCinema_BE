using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Features.Foods.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

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
            return Ok(new PageResponse<IEnumerable<FoodDto>>(
                true,
                "Get foods successfully",
                foods.Data,
                foods.Page,
                foods.Size,
                foods.TotalPages,
                foods.TotalElements));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while getting foods");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById(Guid id)
    {
        try
        {
            var food = await _mediator.Send(new GetFoodByIdQuery { Id = id });
            return Ok(new ApiResponse<FoodDto>(true, "Update food successfully", food));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
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
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while updating food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateFood([FromBody] CreateFoodCommand command)
    {
        try
        {
            var food = await _mediator.Send(command);
            return StatusCode(201, new ApiResponse<FoodDto>(true, "Create food successfully", food));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while creating food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
}