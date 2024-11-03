using BCinema.Application.DTOs;
using BCinema.Application.Features.Payments.Commands;
using FluentValidation;

namespace BCinema.Application.Features.Payments.Validators;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("User Id is required");
        
        RuleFor(x => x.ScheduleId)
            .NotNull().WithMessage("Schedule Id is required");
        
        RuleFor(x => x.PaymentDetails)
            .NotNull().WithMessage("Payment details is required")
            .Must(HaveAtLeastOneSeatId).WithMessage("At least one SeatScheduleId is required in payment details");
        
        RuleForEach(x => x.PaymentDetails).ChildRules(paymentDetail =>
        {
            paymentDetail.RuleFor(x => x.FoodQuantity)
                .NotNull().When(x => x.FoodId.HasValue).WithMessage("Food quantity is required")
                .GreaterThan(0).When(x => x.FoodId.HasValue).WithMessage("Food quantity must be greater than 0");
        });
    }
    
    private static bool HaveAtLeastOneSeatId(IEnumerable<PaymentDetailDto> paymentDetails)
    {
        return paymentDetails.Any(detail => detail.SeatScheduleId.HasValue);
    }
}