using System.ComponentModel.DataAnnotations;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Auth.Commands;

public class VerifyOtpCommand : IRequest<bool>
{
    [Required]
    public string Code { get; set; } = default!;
    
    public class VerifyOtpCommandHandler(
        IOtpRepository otpRepository,
        IUserRepository userRepository) : IRequestHandler<VerifyOtpCommand, bool>
    {
        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var otp = await otpRepository.GetByCodeAsync(request.Code, cancellationToken)
                ?? throw new NotFoundException(nameof(Otp));
            if (otp.ExpireAt < DateTime.UtcNow)
            {
                throw new BadRequestException("OTP is expired");
            }
            var user = otp.User;
            user.IsActivated = true;
            await userRepository.SaveChangesAsync(cancellationToken);
            otpRepository.DeleteAsync(otp);
            return true;
        }
    }
}