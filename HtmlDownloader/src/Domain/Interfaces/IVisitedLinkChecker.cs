namespace Domain.Interfaces;

public interface IVisitedLinkChecker
{
    Task<bool> IsLinkVisited(Uri uri);
}
