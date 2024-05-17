namespace Domain.Services;

public interface IHtmlParser
{
    Task ParseAsync(string pageUrl);
}
