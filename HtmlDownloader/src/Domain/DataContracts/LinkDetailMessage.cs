namespace Domain.DataContracts;

public class LinkDetailMessage
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string Url { get; init; }

    public string? ParentUrl { get; set; }
}
