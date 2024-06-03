using Domain.Interfaces;
using Domain.ValueObjects;
using HtmlAgilityPack;

namespace Application.Services;

public class LinkExtractor : ILinkExtractor
{
    public async Task<IList<Link>> ExtractAsync(Link link, CancellationToken cancellationToken)
    {
        try
        {
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(link.UriString, cancellationToken);
            return ExtractLinks(htmlDoc);
        }
        catch
        {
            return Enumerable.Empty<Link>().ToList();
        }
    }

    private IList<Link> ExtractLinks(HtmlDocument doc)
    {
        var result = new List<Link>();

        var hrefNodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (hrefNodes is null)
        {
            return result;
        }

        foreach (var node in hrefNodes)
        {
            string hrefValue = node.GetAttributeValue("href", string.Empty);
            var link = LinkFactory.Create(hrefValue);
            if (link != null)
            {
                result.Add(link);
            }
        }

        return result;
    }
}
