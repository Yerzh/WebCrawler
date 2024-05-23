using Domain.Interfaces;

namespace Application.Services;

public class UrlFilter : IUrlFilter
{
    private static readonly string[] hrefsFilters = ["mailto:", "tel:", "javascript:", "#"];

    private static readonly string[] allowedSchemes = ["http", "https"];

    public IList<Uri> FilterUrls(IList<string> urls, string baseUrl)
    {
        Uri baseUri = new Uri(baseUrl);

        return urls
            .Where(url =>  !hrefsFilters.Contains(url))
            .Select(url  => convertToAbsoluteUri(baseUri, new Uri(url)))
            .Where(uri => allowedSchemes.Contains(uri.Scheme))
            .ToList();
    }

    private static readonly Func<Uri, Uri, Uri> convertToAbsoluteUri = (baseUri, relativeUri)
        => !relativeUri.IsAbsoluteUri ? new Uri(baseUri, relativeUri) : relativeUri;
}
