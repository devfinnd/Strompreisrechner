namespace FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;

public sealed record CalculateEnergyPriceRequest
{
    public decimal KwhConsumptionPerYear { get; init; }
    public decimal PricePerKwhInEuro { get; init; }
}
