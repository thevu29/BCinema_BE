using BCinema.Application.Features.Foods.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;

namespace BCinema.Application.Features.Foods.Validator
{
    public class CreateFoodValidator : AbstractValidator<CreateFoodCommand>
    {

        public CreateFoodValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        }
    }
}
