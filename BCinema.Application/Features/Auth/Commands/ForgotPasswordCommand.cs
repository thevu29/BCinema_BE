using System.ComponentModel.DataAnnotations;
using BCinema.Application.Enums;
using BCinema.Application.Exceptions;
using BCinema.Application.Mail;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BCinema.Application.Features.Auth.Commands;

public class ForgotPasswordCommand : IRequest<bool>
{
    [Required]
    public string Email { get; set; } = default!;
    
    public class ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IMailService mailService,
        IPasswordHasher<User> passwordHasher,
        ILogger<ForgotPasswordCommandHandler> logger
        ) : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userRepository.GetByEmailAndProviderAsync(request.Email, Provider.Local, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));
                var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                user.Password = passwordHasher.HashPassword(user, newPassword);
                await userRepository.SaveChangesAsync(cancellationToken);
                var mailData = new MailData
                {
                    EmailToId = user.Email,
                    EmailSubject = "Reset Password",
                    EmailBody = "Password: " + newPassword
                };
                return await mailService.SendMailAsync(mailData, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return false;
            }
            
        }
    }
}