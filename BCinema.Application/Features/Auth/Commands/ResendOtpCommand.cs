using BCinema.Application.Exceptions;
using BCinema.Application.Mail;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Auth.Commands;

public class ResendOtpCommand : IRequest<bool>
{
    public Guid UserId { get; set; } = default!;

    public class ResendOtpCommandHandler(
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IMailService mailService) : IRequestHandler<ResendOtpCommand, bool>
    {
        public async Task<bool> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
                       ?? throw new NotFoundException(nameof(User));
            var otp = await otpRepository.GetByUserIdAsync(user.Id, cancellationToken)
                      ?? throw new NotFoundException(nameof(Otp));
            if (otp.Attempts >= 5)
            {
                otpRepository.Delete(otp);
                await otpRepository.SaveChangesAsync(cancellationToken);
                userRepository.Delete(user);
                throw new BadRequestException("You have reached the maximum number of attempts");
            }
            otp.Code = GenerateUtil.GenerateOtp();
            otp.ExpireAt = DateTime.UtcNow.AddMinutes(5);
            otp.Attempts++;
            await otpRepository.SaveChangesAsync(cancellationToken);
            var mailData = new MailData()
            {
                EmailToId = user.Email,
                EmailSubject = "Verify account",
                EmailBody = "Code: " + otp.Code
            };
            await mailService.SendMailAsync(mailData, cancellationToken);
            return true;
        }

        
    }
}