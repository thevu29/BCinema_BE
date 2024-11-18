using BCinema.Application.Enums;
using BCinema.Application.Features.Auth.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Auth.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MustAsync(CheckEmailLocal).WithMessage("Email not exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
        private async Task<bool> CheckEmailLocal(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(u => u.Email == email && u.Provider == Provider.Local, cancellationToken);
        }
    }
}