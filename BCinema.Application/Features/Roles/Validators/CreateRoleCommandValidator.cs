using BCinema.Application.Features.Roles.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Roles.Validators
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MustAsync(BeUniqueName).WithMessage("Role already exists");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return !await _roleRepository.AnyAsync(r => r.Name == name, cancellationToken);
        }
    }
}
