using FluentValidation;
using SmartWater.API.DTOs.Hydrants;

namespace SmartWater.API.Validators;

public class CreateHydrantRequestValidator : AbstractValidator<CreateHydrantRequest>
{
    public CreateHydrantRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(250).WithMessage("Location must not exceed 250 characters.");
    }
}
