using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ILinkFilter
{
    IList<Link> Filter(IList<Link> urls, string baseUrl, CancellationToken cancellationToken);
}
