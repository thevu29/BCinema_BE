using System.ComponentModel.DataAnnotations;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BCinema.Application.Features.Auth.Commands;

public class ForgotPasswordCommand : IRequest<bool>
{
    [Required]
    public string Email { get; set; } = default!;
    
    [Required]
    public string Code { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;
    
    public class ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IOtpRepository otpRepository,
        IPasswordHasher<User> passwordHasher) : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var otp = await otpRepository.GetByCodeAsync(request.Code, cancellationToken)
                      ?? throw new BadRequestException("OTP is invalid");
                
            if (!otp.IsVerified)
            {
                throw new BadRequestException("OTP is not verified");
            }
                
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken)
                       ?? throw new NotFoundException(nameof(User));
            
            user.Password = passwordHasher.HashPassword(user, request.Password);
                
            await userRepository.SaveChangesAsync(cancellationToken);
            await tokenRepository.DeleteByUserIdAsync(user.Id, cancellationToken);
            
            otpRepository.Delete(otp);
            
            return true;
        }
    }
}