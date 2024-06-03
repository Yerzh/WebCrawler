namespace Api.Dtos;

public class DownloadLinkRequest
{
    public required string Type { get; init; }

    public required string Uri { get; init; }
}
