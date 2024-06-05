using Domain.Interfaces;
using Infrastructure.RabbitMQ.Consumers;
using Infrastructure.Redis;
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

                //cfg.Send<DownloadLink>(x =>
                //{
                //    // use customerType for the routing key
                //    x.UseRoutingKeyFormatter(context => context.Message.Type);

                //    // multiple conventions can be set, in this case also CorrelationId
                //    x.UseCorrelationId(context => context.Id);
                //});

                //cfg.Message<DownloadLink>(x => x.SetEntityName("downloadlink"));

                //cfg.Publish<DownloadLink>(x => x.ExchangeType = ExchangeType.Direct);

                cfg.ConfigureEndpoints(context);
            });
        });        

        services.AddSingleton<ILinkVisitTracker, LinkVisitTracker>();

        return services;
    }
}
