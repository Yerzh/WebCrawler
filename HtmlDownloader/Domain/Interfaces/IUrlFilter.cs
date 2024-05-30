namespace Domain.Interfaces;

public interface IUrlFilter
{
    IList<Uri> FilterUrls(IList<string> urls, string baseUrl);
}
