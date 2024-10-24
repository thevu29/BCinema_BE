using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace BCinema.Application.Features.Foods.Validator
{
    public class UpdateFoodValidator : AbstractValidator<UpdateFoodCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateFoodValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .MustAsync(IsNotExistingId).WithMessage("Food not found!");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }

        private async Task<bool> IsNotExistingId(Guid Id, CancellationToken cancellationToken)
        {
            return await _context.Foods.AllAsync(x => x.Id != Id, cancellationToken);
        }
    }
}
