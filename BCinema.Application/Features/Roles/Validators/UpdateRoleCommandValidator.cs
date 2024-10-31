using BCinema.Application.Features.Roles.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Roles.Validators
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            RuleFor(x => x.Name)
                .Must(desc => desc == null || !string.IsNullOrEmpty(desc))
                    .WithMessage("Name cannot be empty")
                .MustAsync(BeUniqueName).WithMessage("Role with the same name already exists");

            RuleFor(x => x.Description)
                .Must(desc => desc == null || !string.IsNullOrEmpty(desc))
                    .WithMessage("Description cannot be empty");
        }

        private async Task<bool> BeUniqueName(
            UpdateRoleCommand command,
            string? name,
            CancellationToken cancellationToken)
        {
            return !await _roleRepository.AnyAsync(x => x.Name == name && x.Id != command.Id, cancellationToken);
        }
    }
}
