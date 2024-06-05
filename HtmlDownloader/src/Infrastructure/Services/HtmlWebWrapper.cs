using Application.Interfaces;
using HtmlAgilityPack;

namespace Infrastructure.Services;

public class HtmlWebWrapper : IHtmlWebWrapper
{
    public async Task<HtmlDocument> LoadFromWebAsync(string uri, CancellationToken cancellationToken)
    {
        var web = new HtmlWeb();
        return await web.LoadFromWebAsync(uri, cancellationToken);
    }
}
