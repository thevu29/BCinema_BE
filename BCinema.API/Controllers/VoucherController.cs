using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.UserVouchers.Commands;
using BCinema.Application.Features.UserVouchers.Queries;
using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Application.Features.Vouchers.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[ApiController]
[Route("api/vouchers")]
public class VoucherController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VoucherController> _logger;

    public VoucherController(IMediator mediator, ILogger<VoucherController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetVouchers([FromQuery] VoucherQuery query)
    {
        try
        {
            var vouchers = await _mediator.Send(new GetAllVoucherQuery { Query = query });
            return Ok(new PageResponse<IEnumerable<VoucherDto>>(true, "Get vouchers successfully",
                vouchers.Data, vouchers.Page, vouchers.Size, vouchers.TotalPages, vouchers.TotalElements));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting all vouchers.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetVoucherByCode(string code)
    {
        try
        {
            var voucher = await _mediator.Send(new GetVoucherByCodeQuery { Code = code });
            return Ok(new ApiResponse<VoucherDto>(true, "Get voucher successfully", voucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpGet("{voucherId}/users/{userId}")]
    public async Task<IActionResult> CheckVoucherUsed(Guid voucherId, Guid userId)
    {
        try
        {
            var userVoucher = await _mediator.Send(
                new GetByUIdAndVIdQuery() { VoucherId = voucherId, UserId = userId });
            return Ok(new ApiResponse<UserVoucherDto>(true, "Check voucher successfully", userVoucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting user-voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpGet("/users/{userId}")]
    public async Task<IActionResult> GetVouchersUsed(Guid userId)
    {
        try
        {
            var userVoucher = await _mediator.Send(
                new GetByUIdAndVIdQuery() { UserId = userId });
            return Ok(new ApiResponse<UserVoucherDto>(true, "Get vouchers used successfully", userVoucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting user-voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVoucher(Guid id, [FromBody] UpdateVoucherCommand command)
    {
        try
        {
            command.Id = id;
            var voucher = await _mediator.Send(command);
            return Ok(new ApiResponse<VoucherDto>(true, "Voucher updated successfully", voucher));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherCommand command)
    {
        try
        {
            var voucher = await _mediator.Send(command);
            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<VoucherDto>(true, "Voucher created successfully", voucher));
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating voucher.");
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }

    [HttpPost("user-voucher")]
    public async Task<IActionResult> SetUserVoucher([FromBody] CreateUserVoucherCommand command)
    {
        try
        {
            var userVoucher = await _mediator.Send(command);
            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<UserVoucherDto>(true, "User voucher created successfully", userVoucher));
        }
        catch (Exception ex) when (ex is ValidationException || ex is BadRequestException)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating user-voucher.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }
}