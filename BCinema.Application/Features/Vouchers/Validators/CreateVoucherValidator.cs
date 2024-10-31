using BCinema.Application.Features.Vouchers.Commands;
using BCinema.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace BCinema.Application.Features.Vouchers.Validators;

public class CreateVoucherValidator : AbstractValidator<CreateVoucherCommand>
{
    private readonly IVoucherRepository _voucherRepository;

    public CreateVoucherValidator(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MustAsync(BeUniqueCode).WithMessage("Code already exists");

        RuleFor(x => x.Discount)
            .NotEmpty().WithMessage("Discount is required.")
            .GreaterThan(0).WithMessage("Discount must be greater than 0");

        RuleFor(x => x.ExpireAt)
            .NotEmpty().WithMessage("ExpireAt is required")
            .GreaterThan(DateTime.Now).WithMessage("ExpireAt must be greater than today");
    }
    
    private async Task<bool> BeUniqueCode(string code, CancellationToken cancellationToken)
    {
        return !await _voucherRepository.AnyAsync(x => x.Code == code, cancellationToken);
    }
}
