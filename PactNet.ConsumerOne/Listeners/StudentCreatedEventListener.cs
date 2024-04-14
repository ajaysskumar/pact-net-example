using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessageBroker;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PactNet.ConsumerOne.Models.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PactNet.Provider.Listeners;

public class StudentCreatedEventListener: BackgroundService
{
    private IModel _channel;
    public StudentCreatedEventListener()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }
    
    private async Task ListenEvent(string queueName, EventHandler<BasicDeliverEventArgs> eventHandler)
    {
        _channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message received: {message}");
            var resultEvent = JsonConvert.DeserializeObject<StudentCreatedEvent>(message);
            // Do anything with the student data
        };
        _channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);
        await Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var resultCreatedEventHandler = new EventHandler<BasicDeliverEventArgs>((model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message received: {message}");
            var resultEvent = JsonConvert.DeserializeObject<StudentCreatedEvent>(message);
        });

        await ListenEvent("student-created", resultCreatedEventHandler);
    }
}