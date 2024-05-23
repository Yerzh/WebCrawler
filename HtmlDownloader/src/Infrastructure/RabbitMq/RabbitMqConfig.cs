namespace Infrastructure.RabbitMq;

public class RabbitMqConfig
{
    public required string HostName { get; init; }

    public required string QueueName { get; init; }
}
