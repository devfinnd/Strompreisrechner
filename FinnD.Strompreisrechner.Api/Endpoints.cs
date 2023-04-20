using FinnD.Strompreisrechner.Api.Database;
using FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;
using FinnD.Strompreisrechner.Api.Features.GetHistory;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinnD.Strompreisrechner.Api;

public static class Endpoints
{
    public static async Task<IResult> Calculate(
        [FromBody] CalculateEnergyPriceRequest request,
        [FromServices] IValidator<CalculateEnergyPriceRequest> validator,
        [FromServices] HistoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (await validator.ValidateAsync(request, cancellationToken) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var historyEntry = new HistoryEntry
        {
            Id = Guid.NewGuid(),
            KwhConsumptionPerYear = request.KwhConsumptionPerYear,
            PricePerKwh = request.PricePerKwh,
            PricePerYear = request.KwhConsumptionPerYear * request.PricePerKwh,
        };

        dbContext.History.Add(historyEntry);
        await dbContext.SaveChangesAsync(cancellationToken);


        return Results.Ok(new CalculateEnergyPricesResponse
        {
            PricePerYear = historyEntry.PricePerYear,
            PricePerMonth = historyEntry.PricePerMonth,
            PricePerDay = historyEntry.PricePerDay,
            PricePerHour = historyEntry.PricePerHour,
            PricePerMinute = historyEntry.PricePerMinute,
        });
    }

    public static async Task<IResult> GetHistory(
        [AsParameters] GetHistoryRequest request,
        [FromServices] HistoryDbContext dbContext,
        [FromServices] IValidator<GetHistoryRequest> validator,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        List<HistoryEntry> result = await dbContext.History
            .AsNoTracking()
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ToListAsync(cancellationToken);

        int totalCount = await dbContext.History.CountAsync(cancellationToken);

        return Results.Ok(new GetHistoryResponse
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Entries = result,
            HasNextPage = request.Page + 1 < (int)Math.Ceiling(totalCount / (double)request.PageSize),
            HasPreviousPage = request.Page > 0
        });
    }
}
