using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker;

public class EventPublisher: IEventPublisher
{
    private IModel _channel;
    public EventPublisher()
    {
        var factory = new ConnectionFactory { HostName = "localhost"};
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        _channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        _channel.BasicPublish(exchange: string.Empty,
            routingKey: queueName,
            basicProperties: null,
            body: body);
        await Task.CompletedTask;
    }
}