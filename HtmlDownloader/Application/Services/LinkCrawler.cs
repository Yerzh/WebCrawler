using Domain.DataContracts;
using Domain.Interfaces;
using Domain.ValueObjects;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class LinkCrawler : ILinkCrawler
{
    private readonly ILinkExtractor _linkExtractor;
    private readonly ILinkFilter _urlFilter;
    private readonly IBus _messageBus;
    private readonly ILinkVisitTracker _visitTracker;
    private readonly ILogger<LinkCrawler> _logger;

    public LinkCrawler(ILinkExtractor linkExtractor,
        ILinkFilter urlFilter,
        IBus messageBus,
        ILinkVisitTracker visitTracker,
        ILogger<LinkCrawler> logger)
    {
        _linkExtractor = linkExtractor;
        _urlFilter = urlFilter;
        _messageBus = messageBus;
        _visitTracker = visitTracker;
        _logger = logger;
    }

    public async Task Crawl(DownloadLink downloadLink, CancellationToken cancellationToken)
    {
        var link = LinkFactory.Create(downloadLink.Uri);
        if (link == null)
        {
            return;
        }

        _logger.LogInformation(link.UriString);
        
        var children = await _linkExtractor.ExtractAsync(link, cancellationToken);
        if (children is null)
        {
            return;
        }

        var baseLink = link.Uri.GetLeftPart(UriPartial.Authority);

        var filteredChildren = _urlFilter.Filter(children, baseLink, cancellationToken);
        if (filteredChildren is null)
        {
            return;
        }

        foreach (var childLink in filteredChildren)
        {
            if (await _visitTracker.ContainsLink(childLink, cancellationToken))
                continue;
            await _visitTracker.TrackLink(link, cancellationToken);

            await _messageBus.Publish(new DownloadLink()
            {
                Type = childLink.Uri.GetLeftPart(UriPartial.Authority),
                Uri = childLink.UriString
            },
            cancellationToken);
        }
    }
}
