using HtmlAgilityPack;

namespace Application.Interfaces;

public interface IHtmlWebWrapper
{
    Task<HtmlDocument> LoadFromWebAsync(string uri, CancellationToken cancellationToken);
}
