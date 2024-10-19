using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Features.Vouchers.Validator;

public class CreateVoucherValidator : AbstractValidator<CreateVoucherCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateVoucherValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(BeUniqueCode).WithMessage("{PropertyName} already exists.");

        RuleFor(x => x.Discount)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.ExpireAt)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be greater than today.");
    }
    
    private async Task<bool> BeUniqueCode(string code, CancellationToken cancellationToken)
    {
        return await _context.Vouchers.AllAsync(x => x.Code != code, cancellationToken);
    }
}