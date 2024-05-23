using Domain.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infrastructure.RabbitMq;

public class RabbitMqConsumer : IMessageConsumer
{
    private readonly RabbitMqConfig _config;

    public RabbitMqConsumer(IOptions<RabbitMqConfig> options)
    {
        _config = options.Value;
    }

    public void ConsumeMessage<T>()
    {
        var factory = new ConnectionFactory { HostName = _config.HostName };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_config.QueueName);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine(message);
        };

        channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);
    }
}
