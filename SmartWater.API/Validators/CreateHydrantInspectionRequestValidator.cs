using FluentValidation;
using SmartWater.API.DTOs.Hydrants;

namespace SmartWater.API.Validators;

public class CreateHydrantInspectionRequestValidator : AbstractValidator<CreateHydrantInspectionRequest>
{
    public CreateHydrantInspectionRequestValidator()
    {
        RuleFor(x => x.Pressure)
            .GreaterThan(0).WithMessage("Pressure must be greater than zero.");

        RuleFor(x => x.FlowRate)
            .GreaterThanOrEqualTo(0).WithMessage("FlowRate cannot be negative.");

        RuleFor(x => x.InspectedBy)
            .NotEmpty().WithMessage("InspectedBy is required.")
            .MaximumLength(100).WithMessage("InspectedBy must not exceed 100 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes is not null);
    }
}
