using Domain.DataContracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MassTransit;

namespace Application.Services;

public class LinkCrawler : ILinkCrawler
{
    private readonly ILinkExtractor _linkExtractor;
    private readonly ILinkFilter _urlFilter;
    private readonly IBus _messageBus;
    private readonly ILinkVisitTracker _visitTracker;

    public LinkCrawler(ILinkExtractor linkExtractor,
        ILinkFilter urlFilter,
        IBus messageBus,
        ILinkVisitTracker visitTracker)
    {
        _linkExtractor = linkExtractor;
        _urlFilter = urlFilter;
        _messageBus = messageBus;
        _visitTracker = visitTracker;
    }

    public async Task Crawl(DownloadLink downloadLink)
    {
        if (downloadLink == null)
        {
            return;
        }

        var link = LinkFactory.Create(downloadLink.Uri);
        if (link == null)
        {
            return;
        }

        if (await _visitTracker.ContainsLink(link))
            return;
        await _visitTracker.TrackLink(link);

        Console.WriteLine(link.UriString);

        CancellationTokenSource cancellationTokenSource = new();
        var children = await _linkExtractor.ExtractAsync(link, cancellationTokenSource.Token);
        if (children is null)
        {
            return;
        }

        var baseLink = link.Uri.GetLeftPart(UriPartial.Authority);
        var filteredChildren = _urlFilter.Filter(children, baseLink);
        if (filteredChildren is null)
        {
            return;
        }

        foreach (var childLink in filteredChildren)
        {
            if (await _visitTracker.ContainsLink(childLink))
                continue;
            await _visitTracker.TrackLink(childLink);

            await _messageBus.Publish(new DownloadLink()
            {
                Type = childLink.Uri.GetLeftPart(UriPartial.Authority),
                Uri = childLink.UriString
            });

            Console.WriteLine(childLink.ToString());
        }
    }
}
