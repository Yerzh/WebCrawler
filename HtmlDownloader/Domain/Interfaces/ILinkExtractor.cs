using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ILinkExtractor
{
    Task<IList<Link>> ExtractAsync(Link link, CancellationToken cancellationToken);
}