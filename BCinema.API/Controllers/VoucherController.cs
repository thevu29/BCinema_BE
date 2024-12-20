﻿using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.UserVouchers.Commands;
using BCinema.Application.Features.UserVouchers.Queries;
using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Application.Features.Vouchers.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[ApiController]
[Authorize]
[Route("api/vouchers")]
public class VoucherController(IMediator mediator, ILogger<VoucherController> logger) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllVouchers()
    {
        try
        {
            var vouchers = await mediator.Send(new GetAllVouchersQuery());
            return Ok(new ApiResponse<IEnumerable<VoucherDto>>(true, "Get all vouchers successfully", vouchers));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rooms");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVouchers([FromQuery] VoucherQuery query)
    {
        try
        {
            var vouchers = await mediator.Send(new GetVouchersQuery { Query = query });

            return Ok(new PageResponse<IEnumerable<VoucherDto>>(
                true,
                "Get vouchers successfully",
                vouchers.Data,
                vouchers.Page,
                vouchers.Size,
                vouchers.TotalPages,
                vouchers.TotalElements));
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
            logger.LogError(ex, "An unexpected error occurred while getting all vouchers");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVoucherById(Guid id)
    {
        try
        {
            var voucher = await mediator.Send(new GetVoucherByIdQuery { Id = id });

            return Ok(new ApiResponse<VoucherDto>(true, "Get voucher successfully", voucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while getting voucher");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetVoucherByCode(string code)
    {
        try
        {
            var voucher = await mediator.Send(new GetVoucherByCodeQuery { Code = code });

            return Ok(new ApiResponse<VoucherDto>(true, "Get voucher successfully", voucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while getting voucher");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpGet("code/{code}/users/{userId:guid}")]
    public async Task<IActionResult> CheckVoucherUsedByCode(Guid userId, string code)
    {
        try
        {
            var userVoucher = await mediator.Send(new GetUserVoucherByUIdAndVCodeQuery() { UserId = userId, Code = code });

            return Ok(new ApiResponse<UseVoucherDto>(true, "Check user used voucher successfully", userVoucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while checking voucher usage");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
    
    [HttpGet("{voucherId:guid}/users/{userId:guid}")]
    public async Task<IActionResult> CheckVoucherUsed(Guid userId, Guid voucherId)
    {
        try
        {
            var userVoucher = await mediator.Send(
                new GetUserVoucherByUIdAndVIdQuery() { UserId = userId, VoucherId = voucherId });

            return userVoucher == null 
                ? Ok(new ApiResponse<string>(true, "User hasn't used this voucher")) 
                : Ok(new ApiResponse<UserVoucherDto>(true, "User has used this voucher", userVoucher));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while checking voucher usage");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserVouchers(Guid userId)
    {
        try
        {
            var userVouchers = await mediator.Send(new GetUserVouchersByUIdQuery { UserId = userId });

            return Ok(new ApiResponse<IEnumerable<UserVoucherDto>>(
                true,
                "Get user vouchers successfully",
                userVouchers));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while getting user vouchers");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherCommand command)
    {
        try
        {
            var voucher = await mediator.Send(command);

            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<VoucherDto>(true, "Voucher created successfully", voucher));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while creating voucher");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpPost("user-voucher")]
    public async Task<IActionResult> UseVoucher([FromBody] CreateUserVoucherCommand command)
    {
        try
        {
            var userVoucher = await mediator.Send(command);
            return StatusCode(
                StatusCodes.Status201Created,
                new ApiResponse<UserVoucherDto>(true, "Use voucher successfully", userVoucher));
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
            logger.LogError(ex, "An unexpected error occurred while using voucher");
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }
}
