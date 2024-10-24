using BCinema.Application.Features.UserVouchers.Commands;
using FluentValidation;

namespace BCinema.Application.Features.UserVouchers.Validator;

public class CreateUserVoucherValidator : AbstractValidator<CreateUserVoucherCommand>
{
    public CreateUserVoucherValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Role id is required.")
            .NotNull();
        
        RuleFor(x => x.VoucherId)
            .NotEmpty().WithMessage("Voucher id is required.")
            .NotNull();
    }
}