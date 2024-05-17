using Domain.Services;
using HtmlAgilityPack;

namespace Infrastructure.Services;

public class HtmlParser : IHtmlParser
{
    public async Task ParseAsync(string pageUrl)
    {
        var web = new HtmlWeb();
        var htmlDoc = await web.LoadFromWebAsync(pageUrl);
        var urls = ExtractUrlsFromPage(htmlDoc);

    }

    private static IList<string> ExtractUrlsFromPage(HtmlDocument doc)
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
