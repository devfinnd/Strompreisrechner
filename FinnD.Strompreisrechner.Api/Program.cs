using System.Net;
using System.Runtime.CompilerServices;
using FinnD.Strompreisrechner.Api.Database;
using FinnD.Strompreisrechner.Api.Features.CalculateEnergyPrice;
using FinnD.Strompreisrechner.Api.Features.GetHistory;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("FinnD.Strompreisrechner.IntegrationTests")]

namespace FinnD.Strompreisrechner.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<HistoryDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("HistoryDb")));
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddSwaggerGen();
        builder.Services.AddEndpointsApiExplorer();

        WebApplication app = builder.Build();

        app.MapPost("/calculate", Endpoints.Calculate)
            .Accepts<CalculateEnergyPriceRequest>("application/json")
            .Produces<CalculateEnergyPricesResponse>((int)HttpStatusCode.OK, "application/json")
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.InternalServerError);

        app.MapGet("/history", Endpoints.GetHistory)
            .Produces<GetHistoryResponse>()
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.InternalServerError);

        app.UseSwagger();
        app.UseSwaggerUI();

        using IServiceScope scope = app.Services.CreateScope();
        {
            await scope.ServiceProvider.GetRequiredService<HistoryDbContext>().Database.EnsureCreatedAsync();
        }

        await app.RunAsync();
    }
}
