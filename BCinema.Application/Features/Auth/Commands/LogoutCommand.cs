using MediatR;
using Microsoft.AspNetCore.Http;

namespace BCinema.Application.Features.Auth.Commands;

public class LogoutCommand : IRequest
{
    public class LogoutCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand>
    {
        public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh-token");
            return Task.FromResult(Unit.Value);
        }
    }
}