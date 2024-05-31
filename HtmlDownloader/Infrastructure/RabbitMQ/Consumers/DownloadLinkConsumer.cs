using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.RabbitMQ.Consumers;

public class DownloadLinkConsumer : IConsumer<DownloadLink>
{
    private readonly ILinkCrawler _crawler;

    public DownloadLinkConsumer(ILinkCrawler crawler)
    {
        _crawler = crawler;
    }

    public async Task Consume(ConsumeContext<DownloadLink> context)
    {
        await _crawler.Crawl(context.Message);
    }
}
