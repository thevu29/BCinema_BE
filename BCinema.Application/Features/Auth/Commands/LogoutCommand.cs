using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest
{
    public class LogoutCommandHandler(ITokenRepository tokenRepository, IHttpContextAccessor httpContextAccessor) 
        : IRequestHandler<LogoutCommand>
    {
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["refresh-token"];
            
            if (refreshToken != null)
            {
                await tokenRepository.DeleteRefreshTokenAsync(refreshToken, cancellationToken);
                httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh-token");
            }
        }
    }
}