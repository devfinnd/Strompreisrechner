using System.Net;
using System.Net.Http.Json;
using FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;
using Xunit.Abstractions;

namespace FinnD.Strompreisrechner.IntegrationTests.Tests;

public class CalculateTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly StrompreisrechnerWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CalculateTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _factory = new StrompreisrechnerWebApplicationFactory(testOutputHelper);
        _client = _factory.CreateDefaultClient();
    }

    [Theory]
    [InlineData(0.40, 3000, 1200)] // Sample 2022
    public async Task Post_ValidCalculateEnergyPrice_Returns_ValidResult(decimal pricePerKwh, decimal kwhConsumptionPerYear, decimal expectedPricePerYear)
    {
        HttpResponseMessage result = await _client.PostAsJsonAsync("/calculate", new CalculateEnergyPriceRequest
        {
            PricePerKwhInEuro = pricePerKwh,
            KwhConsumptionPerYear = kwhConsumptionPerYear
        });

        var response = await result.Content.ReadFromJsonAsync<CalculateEnergyPricesResponse>();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(expectedPricePerYear, response?.PricePerYearInEuro);
    }

    [Theory]
    [InlineData(0.40, 0)]
    [InlineData(0.40, -1)]
    [InlineData(0, 3000)]
    [InlineData(-1, 3000)]
    [InlineData(-1, -1)]
    public async Task Post_InvalidCalculateEnergyPriceRequest_Returns_ValidationProblems(decimal pricePerKwh, decimal kwhConsumptionPerYear)
    {
        HttpResponseMessage result = await _client.PostAsJsonAsync("/calculate", new CalculateEnergyPriceRequest
        {
            PricePerKwhInEuro = pricePerKwh,
            KwhConsumptionPerYear = kwhConsumptionPerYear
        });

        _testOutputHelper.WriteLine(await result.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}
