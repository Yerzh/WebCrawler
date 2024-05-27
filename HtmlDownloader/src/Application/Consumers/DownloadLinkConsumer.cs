using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;
using System.Collections.Concurrent;

namespace Application.Consumers;

public class DownloadLinkConsumer : IConsumer<DownloadLink>
{
    private static ConcurrentBag<Uri> _alreadySeen = new();
    private readonly ILinkExtractor _linkExtractor;
    private readonly IUrlFilter _urlFilter;
    private readonly IPublishEndpoint _publishEndpoint;

    public DownloadLinkConsumer(ILinkExtractor linkExtractor,
        IUrlFilter urlFilter,
        IPublishEndpoint sendEndpoint)
    {
        _linkExtractor = linkExtractor;
        _urlFilter = urlFilter;
        _publishEndpoint = sendEndpoint;
    }

    public async Task Consume(ConsumeContext<DownloadLink> context)
    {
        var link = context.Message;
        if (link == null)
        {
            return;
        }

        var uri = new Uri(link.Url);
        if (_alreadySeen.Contains(uri))
            return;
        _alreadySeen.Add(uri);

        Console.WriteLine(uri.ToString());

        CancellationTokenSource cancellationTokenSource = new();
        var children = await _linkExtractor.ExtractAsync(uri.OriginalString, cancellationTokenSource.Token);
        if (children is null)
        {
            return;
        }

        var baseLink = uri.GetLeftPart(UriPartial.Authority);
        var filteredChildren = _urlFilter.FilterUrls(children, baseLink);
        if (filteredChildren is null)
        {
            return;
        }

        foreach (var childLink in filteredChildren)
        {
            if (_alreadySeen.Contains(childLink))
                continue;
            _alreadySeen.Add(childLink);

            await _publishEndpoint.Publish(new DownloadLink()
            {
                Type = childLink.GetLeftPart(UriPartial.Authority),
                Url = childLink.OriginalString
            });

            Console.WriteLine(childLink.ToString());
        }
    }
}
