namespace Domain.Services;

public interface IUrlFilter
{
    IList<Uri> FilterUrls(IList<string> urls, string baseUrl);
}
