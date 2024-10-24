using BCinema.Application.Features.Foods.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.Features.Foods.Validator
{
    public class DeleteFoodValidator : AbstractValidator<DeleteFoodCommand>
    {
        public DeleteFoodValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
