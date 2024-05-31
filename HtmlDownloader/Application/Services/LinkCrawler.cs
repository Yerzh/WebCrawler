using Domain.DataContracts;
using Domain.Interfaces;
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

    public async Task Crawl(DownloadLink link)
    {
        if (link == null)
        {
            return;
        }

        var uri = new Uri(link.Url);

        if (await _visitTracker.ContainsLink(uri))
            return;
        await _visitTracker.TrackLink(uri);

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
            if (await _visitTracker.ContainsLink(childLink))
                continue;
            await _visitTracker.TrackLink(childLink);

            await _messageBus.Publish(new DownloadLink()
            {
                Type = childLink.GetLeftPart(UriPartial.Authority),
                Url = childLink.OriginalString
            });

            Console.WriteLine(childLink.ToString());
        }
    }
}
