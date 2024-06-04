using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ILinkVisitTracker
{
    Task<bool> ContainsLink(Link link, CancellationToken cancellationToken);

    Task TrackLink(Link link, CancellationToken cancellationToken);
}
