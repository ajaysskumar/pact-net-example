using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PactNet.Provider.Models.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PactNet.Provider.Listeners;

public class ResultCreatedEventListener: BackgroundService
{
    private IModel _channel;
    public ResultCreatedEventListener()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }
    
    public async Task ListenEvent(string queueName, EventHandler<BasicDeliverEventArgs> eventHandler)
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
            var resultEvent = JsonConvert.DeserializeObject<ReportCardCreatedEvent>(message);
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
            var resultEvent = JsonConvert.DeserializeObject<ReportCardCreatedEvent>(message);
        });

        await ListenEvent("result-created", resultCreatedEventHandler);
    }
}