using BCinema.Application.Features.Users.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _userRepository.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}