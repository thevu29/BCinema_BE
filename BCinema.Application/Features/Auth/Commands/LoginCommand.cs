using System.ComponentModel.DataAnnotations;
using BCinema.Application.DTOs.Auth;
using BCinema.Application.Enums;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BCinema.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<JwtResponse>
{
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    
    public class LoginCommandHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        JwtProvider jwtProvider,
        IHttpContextAccessor httpContextAccessor,
        IPasswordHasher<User> passwordHasher) : IRequestHandler<LoginCommand, JwtResponse>
    {
        public async Task<JwtResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAndProviderAsync(request.Email, Provider.Local,
                cancellationToken) ?? throw new BadRequestException("Invalid email");
            if ((bool)(!user.IsActivated)!)
            {
                throw new BadRequestException("Account is not activated");
            }
            if (passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }
            var refreshToken = jwtProvider.GenerateRefreshToken();
            var token = new Token()
            {
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };
            await tokenRepository.AddTokenAsync(token, cancellationToken);
            await tokenRepository.SaveChangesAsync(cancellationToken);
            CookieHelper.SetCookie("refresh-token", refreshToken, httpContextAccessor);
            return new JwtResponse()
            {
                Type = "Bearer",
                AccessToken = jwtProvider.GenerateJwtToken(user)
            };
        }
    }
}