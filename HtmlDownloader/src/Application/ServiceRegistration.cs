using Application.Consumers;
using Application.RabbitMq;
using Application.Services;
using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, RabbitMQConfig rabbitMQConfig)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<DownloadLinkConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                //cfg.Host(rabbitMQConfig.Host, rabbitMQConfig.VirtualHost, h =>
                //{
                //    h.Username(rabbitMQConfig.Username);
                //    h.Password(rabbitMQConfig.Password);
                //});

                //cfg.PrefetchCount = 1;

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

        services.AddSingleton<ILinkExtractor, LinkExtractor>();
        services.AddSingleton<IUrlFilter, UrlFilter>();

        return services;
    }
}
