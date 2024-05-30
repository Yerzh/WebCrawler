namespace Application.RabbitMq;

public class RabbitMQConfig
{
    public required string Host { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string VirtualHost { get; init; }
}
