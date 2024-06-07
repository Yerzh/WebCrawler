using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ILinkFilter
{
    IEnumerable<Link> Filter(IEnumerable<Link> urls, string baseUrl, CancellationToken cancellationToken);
}
