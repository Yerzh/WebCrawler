using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Redis;

namespace HtmlDownloader.IntegrationTests.Helpers;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public readonly RedisContainer RedisContainer =
        new RedisBuilder()
        .WithImage("redis:7.2.5")
        .WithName("redis")
        .WithPortBinding(6379, 6379)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Redis:ConnectionString", RedisContainer.GetConnectionString());

        builder.ConfigureServices(services =>
        {
            services.AddMassTransitTestHarness();
        });
        
        builder.UseEnvironment("Development");
    }
}
