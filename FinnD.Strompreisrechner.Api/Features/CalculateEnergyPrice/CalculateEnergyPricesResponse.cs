namespace FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;

public sealed record CalculateEnergyPricesResponse
{
    public required decimal PricePerYear { get; init; }
    public required decimal PricePerMonth { get; init; }
    public required decimal PricePerDay { get; init; }
    public required decimal PricePerHour { get; init; }
    public required decimal PricePerMinute { get; init; }
}
