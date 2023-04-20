using FinnD.Strompreisrechner.Api;
using FinnD.Strompreisrechner.Api.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace FinnD.Strompreisrechner.IntegrationTests;

public sealed class StrompreisrechnerWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ITestOutputHelper _testOutputHelper;

    public StrompreisrechnerWebApplicationFactory(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
        });

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? dbContextOptions = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<HistoryDbContext>));

            services.Remove(dbContextOptions);

            services.AddDbContext<HistoryDbContext>(opt => opt.UseInMemoryDatabase("TestDb" + Guid.NewGuid()));
        });
    }
}

internal sealed class XUnitLogger<T> : XUnitLogger, ILogger<T>
{
    public XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider)
        : base(testOutputHelper, scopeProvider, typeof(T).FullName)
    {
    }
}
