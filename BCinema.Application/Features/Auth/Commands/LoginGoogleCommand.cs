﻿using BCinema.Application.DTOs.Auth;
using BCinema.Application.Enums;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BCinema.Application.Features.Auth.Commands;

public class LoginGoogleCommand : IRequest<JwtResponse>
{
    public string IdToken { get; set; } = default!;
    
    public class LoginGoogleCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITokenRepository tokenRepository,
        JwtProvider jwtProvider,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<LoginGoogleCommand, JwtResponse>
    {
        private readonly string? _googleClientId = configuration["GoogleConfig:ClientId"];
        
        public async Task<JwtResponse> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationGoogle = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_googleClientId]
                };
                
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, validationGoogle);
                var user = await userRepository.GetByEmailAndProviderAsync(payload.Email, Provider.Google, cancellationToken);
                
                if (user == null)
                {
                    user = new User()
                    {
                        Email = payload.Email,
                        Name = payload.Name,
                        Avatar = payload.Picture,
                        Provider = Provider.Google,
                        Password = null,
                        IsActivated = true
                    };
                    
                    var role = await roleRepository.GetByNameAsync("User", cancellationToken) 
                               ?? throw new NotFoundException(nameof(Role));
                    
                    user.Role = role;
                    await userRepository.AddAsync(user, cancellationToken);
                }
                else
                {
                    if (string.IsNullOrEmpty(user.Name))
                    {
                        user.Name = payload.Name;
                    }

                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        user.Avatar = payload.Picture;
                    }
                }

                await userRepository.SaveChangesAsync(cancellationToken);

                var refreshToken = JwtProvider.GenerateRefreshToken();
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
            catch (InvalidJwtException e)
            {
                throw new Exception("Invalid ID token.", e);
            }
            
        }
    }
}