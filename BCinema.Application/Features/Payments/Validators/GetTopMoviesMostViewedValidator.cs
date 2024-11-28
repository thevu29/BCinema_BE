using BCinema.Application.Features.Payments.Queries;
using FluentValidation;

namespace BCinema.Application.Features.Payments.Validators;

public class GetTopMoviesMostViewedValidator : AbstractValidator<GetTopMoviesMostViewedQuery>
{
    public GetTopMoviesMostViewedValidator()
    {
        RuleFor(x => x.Count)
            .NotEmpty().WithMessage("Count is required")
            .GreaterThan(0).WithMessage("Count must be greater than 0");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required")
            .GreaterThan(0).WithMessage("Year must be greater than 0");

        RuleFor(x => x.Month)
            .NotEmpty().WithMessage("Month is required")
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12");
    }
}