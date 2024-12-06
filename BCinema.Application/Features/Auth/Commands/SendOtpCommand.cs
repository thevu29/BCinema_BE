using System.ComponentModel.DataAnnotations;
using BCinema.Application.Enums;
using BCinema.Application.Exceptions;
using BCinema.Application.Mail;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Auth.Commands;

public class SendOtpCommand : IRequest<bool>
{
    [Required]
    public string Email { get; set; } = default!;
    
    public class SendOtpCommandHandler(
        IUserRepository userRepository,
        IMailService mailService,
        IOtpRepository otpRepository
        ) : IRequestHandler<SendOtpCommand, bool>
    {
        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAndProviderAsync(request.Email, Provider.Local, cancellationToken)
                       ?? throw new NotFoundException(nameof(User));
            
            if ((bool)(!user.IsActivated)!)
            {
                throw new BadRequestException("Account is not activated");
            }
            
            var otp = new Otp
            {
                UserId = user.Id,
                Code = GenerateUtil.GenerateOtp(),
                ExpireAt = DateTime.UtcNow.AddMinutes(5)
            };
            
            await otpRepository.AddAsync(otp, cancellationToken);
            await otpRepository.SaveChangesAsync(cancellationToken);
            
            var mailData = new MailData()
            {
                EmailToId = user.Email,
                EmailSubject = "Verify forgot password",
                EmailBody = "Code: " + otp.Code
            };
            
            await mailService.SendMailAsync(mailData, cancellationToken);
            return true;
        }
    }
}