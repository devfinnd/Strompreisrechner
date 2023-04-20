using FinnD.Strompreisrechner.Api.Database;

namespace FinnD.Strompreisrechner.Api.Features.GetHistory;

public sealed record GetHistoryResponse
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
    public required int TotalPages { get; init; }
    public required List<HistoryEntry> Entries { get; init; }

    public required bool HasPreviousPage { get; init; }
    public required bool HasNextPage { get; init; }
}
