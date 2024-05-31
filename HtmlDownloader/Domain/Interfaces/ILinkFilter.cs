namespace Domain.Interfaces;

public interface ILinkFilter
{
    IList<Uri> Filter(IList<string> urls, string baseUrl);
}
