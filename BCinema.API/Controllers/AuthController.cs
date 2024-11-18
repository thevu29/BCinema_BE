using BCinema.API.Responses;
using BCinema.Application.DTOs;
using BCinema.Application.DTOs.Auth;
using BCinema.Application.Exceptions;
using BCinema.Application.Features.Auth.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCinema.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IMediator mediator, ILogger<FoodController> logger) : ControllerBase
{
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var jwtResponse = await mediator.Send(command);
            return Ok(new ApiResponse<JwtResponse>(true, "Login successfully", jwtResponse));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }    
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("login/oauth2/code/google")]
    public async Task<IActionResult> LoginGoogle([FromBody] LoginGoogleCommand command)
    {
        try
        {
            var jwtResponse = await mediator.Send(command);
            return Ok(new ApiResponse<JwtResponse>(true, "Login successfully", jwtResponse));
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
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var user = await mediator.Send(command);
            return Ok(new ApiResponse<UserDto>(true, "Register successfully", user));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var jwtResponse = await mediator.Send(new RefreshTokenCommand());
            return Ok(new ApiResponse<JwtResponse>(true, "Token refreshed successfully", jwtResponse));
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
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        try
        {
            bool resp = await mediator.Send(command);
            return Ok(new ApiResponse<bool>(resp, resp ? "Password reset successfully" : "Password reset failed", resp));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiResponse<string>(false, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command)
    {
        try
        {
            bool resp = await mediator.Send(command);
            return Ok(new ApiResponse<bool>(resp, resp ? "OTP verified successfully" : "OTP verified fail", resp));
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
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
    
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpCommand command)
    {
        try
        {
            bool resp = await mediator.Send(command);
            return Ok(new ApiResponse<bool>(resp, resp ? "OTP resent successfully" : "OTP resent fail", resp));
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
            logger.LogError(ex, "An error occured while getting food");
            return StatusCode(500, new ApiResponse<string>(false, "An error occured"));
        }
    }
}