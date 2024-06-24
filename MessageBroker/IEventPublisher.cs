namespace MessageBroker;

public interface IEventPublisher
{
    Task PublishAsync<T>(T message, string queueName);
}