using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;

public class LinkFilter : ILinkFilter
{
    private static readonly string[] hrefsFilters = ["mailto:", "tel:", "javascript:", "#"];

    private static readonly string[] allowedSchemes = ["http", "https"];

    public IList<Link> Filter(IList<Link> links, string baseUrl, CancellationToken cancellationToken)
    {
        Uri baseUri = new Uri(baseUrl);

        return links
            .Where(link =>  !hrefsFilters.Contains(link.UriString))
            .Select(link  => convertToAbsoluteUri(baseUri, link))
            .Where(link => allowedSchemes.Contains(link.Uri.Scheme))
            .ToList();
    }

    private static readonly Func<Uri, Link, Link> convertToAbsoluteUri = (baseUri, relativeLink)
        => !relativeLink.Uri.IsAbsoluteUri ? new Link(baseUri, relativeLink.UriString) : relativeLink;
}
