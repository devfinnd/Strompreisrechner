using FluentValidation;

namespace FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;

public sealed class CalculateEnergyPriceRequestValidator : AbstractValidator<CalculateEnergyPriceRequest>
{
    public CalculateEnergyPriceRequestValidator()
    {
        RuleFor(x => x.KwhConsumptionPerYear)
            .GreaterThan(0)
            .WithMessage("KWh consumption per year must be greater than 0");
        RuleFor(x => x.PricePerKwhInEuro)
            .GreaterThan(0)
            .WithMessage("Price per kWh must be greater than 0");
    }
}
