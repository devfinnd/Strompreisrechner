using System.Net;
using System.Net.Http.Json;
using FinnD.Strompreisrechner.Api.Database;
using FinnD.Strompreisrechner.Api.Features.GetHistory;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace FinnD.Strompreisrechner.IntegrationTests.Tests;

public sealed class PaginationTests
{
    private readonly StrompreisrechnerWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PaginationTests(ITestOutputHelper testOutputHelper)
    {
        _factory = new StrompreisrechnerWebApplicationFactory(testOutputHelper);
        _client = _factory.CreateDefaultClient();
    }

    [Fact]
    public async Task Get_History_Returns_Results()
    {
        using (IServiceScope services = _factory.Services.CreateScope())
        {
            var db = services.ServiceProvider.GetRequiredService<HistoryDbContext>();
            db.History.AddRange(
                new HistoryEntry
                {
                    Id = Guid.NewGuid(),
                    PricePerKwhInEuro = 0.40m,
                    KwhConsumptionPerYear = 3000,
                    PricePerYearInEuro = 1200,
                    CreatedAt = DateTime.UtcNow
                }, new HistoryEntry
                {
                    Id = Guid.NewGuid(),
                    PricePerKwhInEuro = 0.10m,
                    KwhConsumptionPerYear = 3000,
                    PricePerYearInEuro = 300,
                    CreatedAt = DateTime.UtcNow
                });

            await db.SaveChangesAsync();
        }

        HttpResponseMessage result = await _client.GetAsync("/history?page=1&pageSize=10");
        var response = await result.Content.ReadFromJsonAsync<GetHistoryResponse>();

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(2, response.Entries.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Equal(1, response.Page);
        Assert.Equal(10, response.PageSize);
        Assert.False(response.HasNextPage, "Response should not have a next page");
        Assert.False(response.HasPreviousPage, "Response should not have a previous page");
    }
}
