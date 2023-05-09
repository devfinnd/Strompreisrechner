using FinnD.Strompreisrechner.Api.Database;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinnD.Strompreisrechner.Api.Features.GetHistory;

public sealed class GetHistoryEndpoint
{
    public static async Task<IResult> Handler(
        [AsParameters] GetHistoryRequest request,
        [FromServices] HistoryDbContext dbContext,
        [FromServices] IValidator<GetHistoryRequest> validator,
        [FromServices] ILogger<GetHistoryEndpoint> logger,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Validating request {Request}", request);

        if (await validator.ValidateAsync(request, cancellationToken) is { IsValid: false } validationResult)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        logger.LogInformation("Fetching History for Page {Page} with size {Size}", request.Page, request.PageSize);

        List<HistoryEntry> result = await dbContext.History
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
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
            HasPreviousPage = request.Page > 1
        });
    }
}
