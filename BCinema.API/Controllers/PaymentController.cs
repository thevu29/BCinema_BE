using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Payments.Commands;
using BCinema.Application.Features.Payments.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/payments")]
// [Authorize]
[ApiController]
public class PaymentController(IMediator mediator, ILogger<PaymentController> logger) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPaymentById(Guid id)
    {
        try
        {
            var payment = await mediator.Send(new GetPaymentByIdQuery { Id = id });
            return Ok(new ApiResponse<PaymentDto>(true, "Get payment successfully", payment));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting payment");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred while getting payment"));
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPayments([FromQuery] PaymentQuery query)
    {
        try
        {
            var payments = await mediator.Send(new GetPaymentsQuery { Query = query });
            return Ok(new PageResponse<IEnumerable<PaymentDto>>(
                true,
                "Get payments successfully",
                payments.Data,
                payments.Page,
                payments.Size,
                payments.TotalPages,
                payments.TotalElements));
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
            logger.LogError(ex, "An error occurred while getting payments");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred while getting payments"));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await mediator.Send(command, cancellationToken);
            return StatusCode(201, new ApiResponse<PaymentDto>(true, "Payment created successfully", payment));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a payment");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred while creating a payment"));
        }
    }
    
    [HttpPost("momo")]
    public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentInfoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var paymentUrl = await mediator.Send(command, cancellationToken);
            return Ok(new ApiResponse<string>(true, "Payment url created successfully", paymentUrl));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a payment url");
            return StatusCode(500, new ApiResponse<string>(false, "An error occurred while creating a payment url"));
        }
    }
    
    [HttpGet("momo/callback")]
    public async Task<IActionResult> MomoCallback([FromQuery] MomoCallbackCommand command, CancellationToken cancellationToken)
    {
        var redirectUrl = $"http://localhost:3000/order-status?orderId={command.OrderId}&result_code=";
        try
        {
            var queryParams = HttpContext.Request.Query;
            if (queryParams["resultCode"] != "0")
            {
                command.ErrorCode = queryParams["resultCode"]!;
            }
            var resp = await mediator.Send(command, cancellationToken);
            return Redirect(redirectUrl + resp);
        }
        catch (NotFoundException ex)
        {
            return Redirect(redirectUrl + "404");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing momo callback");
            return Redirect(redirectUrl + "500");
        }
    }
}