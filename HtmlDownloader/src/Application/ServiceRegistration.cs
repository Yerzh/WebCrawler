using Domain.DataContracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.Send<DownloadLink>(x =>
                {
                    x.UseRoutingKeyFormatter(context => context.Message.Type);
                });

                cfg.Message<DownloadLink>(x => x.SetEntityName("download-link"));
                
                cfg.Publish<DownloadLink>(x => x.ExchangeType = ExchangeType.Direct);
            });
        });

        return services;
    }
}
