using Api.Dtos;
using Domain.DataContracts;

namespace Api.Mappers;

public static class DownloadLinkMapper
{
    public static DownloadLink MapToContract(this DownloadLinkRequest link)
    {
        return new DownloadLink()
        {
            Type = link.Type,
            Url = link.Url
        };
    }
}
