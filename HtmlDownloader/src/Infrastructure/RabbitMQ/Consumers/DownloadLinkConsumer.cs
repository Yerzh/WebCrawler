using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.RabbitMQ.Consumers;

public class DownloadLinkConsumer : IConsumer<DownloadLink>
{
    private readonly ILinkCrawler _crawler;
    private readonly IPolitenessPolicyProvider _politenessPolicy;

    public DownloadLinkConsumer(ILinkCrawler crawler,
        IPolitenessPolicyProvider politenessPolicy)
    {
        _crawler = crawler;
        _politenessPolicy = politenessPolicy;
    }

    public async Task Consume(ConsumeContext<DownloadLink> context)
    {
        if (context.Message is null)
        {
            return;
        }

        await _politenessPolicy.ApplyPolicy();

        CancellationTokenSource cancellationTokenSource = new();

        await _crawler.Crawl(context.Message, cancellationTokenSource.Token);
    }
}
