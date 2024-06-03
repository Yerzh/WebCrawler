using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ILinkVisitTracker
{
    Task<bool> ContainsLink(Link link);

    Task TrackLink(Link link);
}
