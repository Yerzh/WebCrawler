using Domain.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.RabbitMq;

public class RabbitMQProducer : IMessageProducer
{
    private readonly RabbitMqConfig _config;

    public RabbitMQProducer(IOptions<RabbitMqConfig> options)
    {
        _config = options.Value;
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = _config.HostName };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(_config.QueueName);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: _config.QueueName, body: body);
    }
}
