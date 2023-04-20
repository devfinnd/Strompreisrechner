namespace FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;

public sealed record CalculateEnergyPricesResponse
{
    public required decimal PricePerYearInEuro { get; init; }
    public required decimal PricePerMonthInEuro { get; init; }
    public required decimal PricePerDayInEuro { get; init; }
    public required decimal PricePerHourInEuro { get; init; }
    public required decimal PricePerMinuteInEuro { get; init; }
}
