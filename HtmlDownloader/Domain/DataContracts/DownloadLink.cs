namespace Domain.DataContracts;

public record DownloadLink
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Type { get; init; }

    public required string Uri { get; init; }
}
