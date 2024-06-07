using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;

public class LinkFilter : ILinkFilter
{
    private static readonly string[] allowedSchemes = ["http", "https"];

    public IEnumerable<Link> Filter(IEnumerable<Link> links, string baseUrl, CancellationToken cancellationToken)
    {
        Uri baseUri = new Uri(baseUrl);

        return links
            .Select(link  => convertToAbsoluteUri(baseUri, link))
            .Where(link => allowedSchemes.Contains(link.Uri.Scheme))
            .Where(link => !link.UriString.Contains('#'))
            .ToList();
    }

    private static readonly Func<Uri, Link, Link> convertToAbsoluteUri = (baseUri, relativeLink)
        => !relativeLink.Uri.IsAbsoluteUri ? new Link(baseUri, relativeLink.UriString) : relativeLink;
}
