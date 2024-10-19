using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;

namespace BCinema.Application.Features.Vouchers.Validator;

public class UpdateVoucherValidator : AbstractValidator<UpdateVoucherCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVoucherValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.Discount)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.ExpireAt)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be greater than today.");
    }
}