namespace FinnD.Strompreisrechner.Api.Features.GetHistory;

public sealed record GetHistoryRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; }
}
