namespace Domain.Interfaces;

public interface ILinkExtractor
{
    Task<IList<string>> ExtractAsync(string pageUrl, CancellationToken cancellationToken);
}