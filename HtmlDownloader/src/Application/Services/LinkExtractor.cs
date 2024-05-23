using Domain.Interfaces;
using HtmlAgilityPack;

namespace Application.Services;

public class LinkExtractor : ILinkExtractor
{
    public async Task<IList<string>> ExtractAsync(string pageUrl, CancellationToken cancellationToken)
    {
        try
        {
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(pageUrl, cancellationToken);
            return ExtractLinks(htmlDoc);
        }
        catch
        {
            return Enumerable.Empty<string>().ToList();
        }
    }

    private IList<string> ExtractLinks(HtmlDocument doc)
    {
        var result = new List<string>();

        var hrefNodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (hrefNodes is null)
        {
            return result;
        }

        foreach (var node in hrefNodes)
        {
            string hrefValue = node.GetAttributeValue("href", string.Empty);
            result.Add(hrefValue);
        }

        return result;
    }
}
