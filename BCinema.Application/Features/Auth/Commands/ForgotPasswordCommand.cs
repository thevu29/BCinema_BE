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
    
    [Required]
    public string Code { get; set; } = default!;
    
    public class ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IMailService mailService,
        ITokenRepository tokenRepository,
        IOtpRepository otpRepository,
        IPasswordHasher<User> passwordHasher,
        ILogger<ForgotPasswordCommandHandler> logger
        ) : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var otp = await otpRepository.GetByCodeAsync(request.Code, cancellationToken)
                    ?? throw new BadRequestException("OTP is invalid");
                if (!otp.IsVerified)
                {
                    throw new BadRequestException("OTP is not verified");
                }
                var user = await userRepository.GetByIdAsync(otp.UserId, cancellationToken)
                    ?? throw new NotFoundException(nameof(User));
                if (user.Email != request.Email)
                {
                    throw new BadRequestException("Email is invalid");
                }
                var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
                user.Password = passwordHasher.HashPassword(user, newPassword);
                await userRepository.SaveChangesAsync(cancellationToken);
                await tokenRepository.DeleteByUserIdAsync(user.Id, cancellationToken);
                otpRepository.Delete(otp);
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