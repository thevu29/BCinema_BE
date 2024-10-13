using BCinema.Application.Features.Users.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Users.Validator
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserCommandValidator(IApplicationDbContext context)
        {
            _context = context;

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
            return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}