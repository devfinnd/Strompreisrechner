using FinnD.Strompreisrechner.Api.Database;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;

public sealed class CalculateEnergyPriceEndpoint
{
    public static async Task<IResult> Handler(
        [FromBody] CalculateEnergyPriceRequest request,
        [FromServices] IValidator<CalculateEnergyPriceRequest> validator,
        [FromServices] HistoryDbContext dbContext,
        [FromServices] ILogger<CalculateEnergyPriceEndpoint> logger,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Validating request {Request}", request);

        if (await validator.ValidateAsync(request, cancellationToken) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        logger.LogInformation("Writing history entry");

        var historyEntry = new HistoryEntry
        {
            Id = Guid.NewGuid(),
            KwhConsumptionPerYear = request.KwhConsumptionPerYear,
            PricePerKwhInEuro = request.PricePerKwhInEuro,
            PricePerYearInEuro = request.KwhConsumptionPerYear * request.PricePerKwhInEuro,
        };

        dbContext.History.Add(historyEntry);

        logger.LogInformation("Saving history");

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new CalculateEnergyPricesResponse
        {
            PricePerYearInEuro = historyEntry.PricePerYearInEuro,
            PricePerMonthInEuro = historyEntry.PricePerMonthInEuro,
            PricePerDayInEuro = historyEntry.PricePerDayInEuro,
            PricePerHourInEuro = historyEntry.PricePerHourInEuro,
            PricePerMinuteInEuro = historyEntry.PricePerMinuteInEuro,
        });
    }
}
