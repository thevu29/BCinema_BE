using BCinema.Application.Features.Foods.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Foods.Validators;

public class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
{
    public UpdateFoodCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => name == null || !string.IsNullOrEmpty(name)).WithMessage("Name can not be empty");

        RuleFor(x => x.Price)
            .Must(price => price is null or > 0).WithMessage("Price must be greater than 0");
    }
}