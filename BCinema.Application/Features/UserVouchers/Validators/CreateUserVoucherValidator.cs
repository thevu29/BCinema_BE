using BCinema.Application.Features.UserVouchers.Commands;
using FluentValidation;

namespace BCinema.Application.Features.UserVouchers.Validators;

public class CreateUserVoucherValidator : AbstractValidator<CreateUserVoucherCommand>
{
    public CreateUserVoucherValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required");
    }
}