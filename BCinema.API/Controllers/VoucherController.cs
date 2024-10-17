﻿using BCinema.API.Responses;
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
    public async Task<IActionResult> GetVouchers()
    {
        try
        {
            var vouchers = await _mediator.Send(new GetAllVoucherQuery());
            return Ok(new ApiResponse<IEnumerable<VoucherDto>>(true, "Get all voucher successfully", vouchers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting all roles.");
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
            _logger.LogError(ex, "An unexpected error occurred while creating role.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
        
    }
    
    [HttpGet("{voucherId}/users/{userId}")]
    public async Task<IActionResult> GetUserVoucher(Guid voucherId, Guid userId)
    {
        try
        {
            var userVoucher = await _mediator.Send(new GetByUIdAndVIdQuery() { VoucherId = voucherId, UserId = userId });
            return Ok(new ApiResponse<UserVoucherDto>(true, "Get user voucher successfully", userVoucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating role.");
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
            _logger.LogError(ex, "An unexpected error occurred while creating role.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherCommand command)
    {
        try
        {
            var voucher = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<VoucherDto>(true, "Voucher created successfully", voucher));
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
    
    [HttpPost("user-voucher")]
    public async Task<IActionResult> SetUserVoucher([FromBody] CreateUserVoucherCommand command)
    {
        try
        {
            var userVoucher = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<UserVoucherDto>(true, "User voucher created successfully", userVoucher));
        }
        catch (Exception ex) when (ex is ValidationException || ex is BadRequestException )
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating role.");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred."));
        }
    }
}