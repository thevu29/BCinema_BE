using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Enums;
using BCinema.Application.Exceptions;
using BCinema.Application.Mail;
using BCinema.Application.Utils;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BCinema.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<UserDto>
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    
    public class RegisterCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IOtpRepository otpRepository,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        IMailService mailService) : IRequestHandler<RegisterCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Provider = Provider.Local
            };
            user.Password = passwordHasher.HashPassword(user, request.Password);
            var role = await roleRepository.GetByNameAsync("User", cancellationToken)
                ?? throw new NotFoundException(nameof(Role));
            user.Role = role;
            await userRepository.AddAsync(user, cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);
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
                EmailSubject = "Verify account",
                EmailBody = "Code: " + otp.Code
            };
            await mailService.SendMailAsync(mailData, cancellationToken);
            return mapper.Map<UserDto>(user);
        }
    }
}