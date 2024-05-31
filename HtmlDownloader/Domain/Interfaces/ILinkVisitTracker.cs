namespace Domain.Interfaces;

public interface ILinkVisitTracker
{
    Task<bool> ContainsLink(Uri uri);

    Task TrackLink(Uri uri);
}
