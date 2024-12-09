using BCinema.Application.Features.Foods.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Foods.Validators;

public class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
{
    public CreateFoodCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        
        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required");
    }
}