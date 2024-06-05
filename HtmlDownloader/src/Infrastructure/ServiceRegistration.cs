using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.RabbitMQ.Consumers;
using Infrastructure.Redis;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<DownloadLinkConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.PrefetchCount = 1;

                cfg.ConfigureEndpoints(context);
            });
        });        

        services.AddSingleton<ILinkVisitTracker, LinkVisitTracker>();
        services.AddSingleton<IHtmlWebWrapper, HtmlWebWrapper>();

        return services;
    }
}
