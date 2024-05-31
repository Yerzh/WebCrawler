using Domain.DataContracts;
using Domain.Interfaces;
using MassTransit;
using System.Collections.Concurrent;

namespace Application.Services;

public class LinkCrawler : ILinkCrawler
{
    private static ConcurrentBag<Uri> _alreadySeen = new();
    private readonly ILinkExtractor _linkExtractor;
    private readonly ILinkFilter _urlFilter;
    private readonly IBus _messageBus;

    public LinkCrawler(ILinkExtractor linkExtractor,
        ILinkFilter urlFilter,
        IBus messageBus)
    {
        _linkExtractor = linkExtractor;
        _urlFilter = urlFilter;
        _messageBus = messageBus;
    }

    public async Task Crawl(DownloadLink link)
    {
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
        var filteredChildren = _urlFilter.Filter(children, baseLink);
        if (filteredChildren is null)
        {
            return;
        }

        foreach (var childLink in filteredChildren)
        {
            if (_alreadySeen.Contains(childLink))
                continue;
            _alreadySeen.Add(childLink);

            await _messageBus.Publish(new DownloadLink()
            {
                Type = childLink.GetLeftPart(UriPartial.Authority),
                Url = childLink.OriginalString
            });

            Console.WriteLine(childLink.ToString());
        }
    }
}
