namespace FinnD.Strompreisrechner.Api.Database;

public sealed class HistoryEntry
{
    public Guid Id { get; set; }
    public decimal KwhConsumptionPerYear { get; set; }
    public decimal PricePerKwhInEuro { get; set; }

    public decimal PricePerYearInEuro { get; set; }
    public decimal PricePerMonthInEuro => PricePerYearInEuro / 12;
    public decimal PricePerDayInEuro => PricePerYearInEuro / 365;
    public decimal PricePerHourInEuro => PricePerDayInEuro / 24;
    public decimal PricePerMinuteInEuro => PricePerHourInEuro / 60;

    public DateTime CreatedAt { get; set; }
}
