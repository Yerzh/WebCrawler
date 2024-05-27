using Application.Consumers;
using Application.Services;
using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<DownloadLinkConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.PrefetchCount = 1;

                cfg.Send<DownloadLink>(x =>
                {
                    // use customerType for the routing key
                    x.UseRoutingKeyFormatter(context => context.Message.Type);

                    // multiple conventions can be set, in this case also CorrelationId
                    x.UseCorrelationId(context => context.Id);
                });

                // Keeping in mind that the default exchange config for your published type will be the full typename of your message
                // we explicitly specify which exchange the message will be published to. So it lines up with the exchange we are binding our
                // consumers too.
                cfg.Message<DownloadLink>(x => x.SetEntityName("download_link"));

                // Also if your publishing your message: because publishing a message will, by default, send it to a fanout queue.
                // We specify that we are sending it to a direct queue instead. In order for the routingkeys to take effect.
                cfg.Publish<DownloadLink>(x => x.ExchangeType = ExchangeType.Direct);
            });
        });

        services.AddSingleton<ILinkExtractor, LinkExtractor>();
        services.AddSingleton<IUrlFilter, UrlFilter>();

        return services;
    }
}
