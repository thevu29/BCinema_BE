using BCinema.Application.DTOs.Auth;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<JwtResponse>
{
    public class RefreshTokenCommandHandler(
        JwtProvider jwtProvider,
        ITokenRepository tokenRepository,
        IRoleRepository roleRepository,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<RefreshTokenCommand, JwtResponse>
    {
        public async Task<JwtResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["refresh-token"]
                ?? throw new BadRequestException("Refresh token not exists");
            var token = await tokenRepository.GetTokenByRefreshTokenAsync(refreshToken, cancellationToken)
                ?? throw new BadRequestException("Refresh token not exists");
            if (token.RefreshTokenExpireAt < DateTime.UtcNow)
            {
                throw new BadRequestException("Refresh token is expired");
            }
            var user = token.User;
            var role = await roleRepository.GetByIdAsync(user.RoleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Role));
            user.Role = role;
            
            token.RefreshToken = jwtProvider.GenerateRefreshToken();
            await tokenRepository.SaveChangesAsync(cancellationToken);
            CookieHelper.SetCookie("refresh-token", token.RefreshToken, httpContextAccessor);
            return new JwtResponse()
            {
                Type = "Bearer",
                AccessToken = jwtProvider.GenerateJwtToken(user)
            };
        }
    }
}