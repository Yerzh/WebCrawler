namespace Domain.Services;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}

