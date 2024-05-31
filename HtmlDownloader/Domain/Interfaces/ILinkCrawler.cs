using Domain.DataContracts;

namespace Domain.Interfaces;

public interface ILinkCrawler
{
    Task Crawl(DownloadLink link);
}
