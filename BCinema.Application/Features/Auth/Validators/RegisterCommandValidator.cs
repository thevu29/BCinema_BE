using BCinema.Application.Enums;
using BCinema.Application.Features.Auth.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Auth.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MustAsync(BeUniqueEmailLocal).WithMessage("Email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }

        private async Task<bool> BeUniqueEmailLocal(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.AnyAsync(u => u.Email == email && u.Provider == Provider.Local, cancellationToken);
        }
    }
}