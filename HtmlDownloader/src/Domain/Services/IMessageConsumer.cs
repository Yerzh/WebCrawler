namespace Domain.Services;

public interface IMessageConsumer
{
    void ConsumeMessage<T>();
}
