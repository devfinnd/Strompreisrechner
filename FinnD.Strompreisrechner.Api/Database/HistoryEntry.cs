namespace FinnD.Strompreisrechner.Api.Database;

public sealed class HistoryEntry
{
    public Guid Id { get; set; }
    public decimal KwhConsumptionPerYear { get; set; }
    public decimal PricePerKwh { get; set; }

    public decimal PricePerYear { get; set; }
    public decimal PricePerMonth => PricePerYear / 12;
    public decimal PricePerDay => PricePerYear / 365;
    public decimal PricePerHour => PricePerDay / 24;
    public decimal PricePerMinute => PricePerHour / 60;

    public DateTime CreatedAt { get; set; }
}
