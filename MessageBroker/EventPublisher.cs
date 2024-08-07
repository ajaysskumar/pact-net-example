﻿using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageBroker;

public class EventPublisher: IEventPublisher
{
    private readonly IModel _channel;
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

public class FakeEventPublisher: IEventPublisher
{
    public async Task PublishAsync<T>(T message, string queueName)
    {
        Console.WriteLine($"Event published on {queueName}");
        await Task.CompletedTask;
    }
}