using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ILinkExtractor, LinkExtractor>();
        services.AddSingleton<ILinkFilter, LinkFilter>();
        services.AddSingleton<ILinkCrawler, LinkCrawler>();

        return services;
    }
}
