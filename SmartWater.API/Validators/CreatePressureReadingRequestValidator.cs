using FluentValidation;
using SmartWater.API.DTOs.Pressure;

namespace SmartWater.API.Validators;

public class CreatePressureReadingRequestValidator : AbstractValidator<CreatePressureReadingRequest>
{
    public CreatePressureReadingRequestValidator()
    {
        RuleFor(x => x.Sector)
            .NotEmpty().WithMessage("Sector is required.")
            .MaximumLength(100).WithMessage("Sector must not exceed 100 characters.");

        RuleFor(x => x.Value)
            .InclusiveBetween(15, 30).WithMessage("Pressure value must be within operational range (15–30 mca).");
    }
}
