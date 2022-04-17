using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NetCoreRabbitMQ.Web.Models;
using NetCoreRabbitMQ.Web.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetCoreRabbitMQ.Web.BackgroundServices
{
    public class InvoiceSenderBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMQClientService;
        private IModel _channel;

        public InvoiceSenderBackgroundService(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMQClientService = rabbitMqClientService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect();
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(queue: RabbitMQClientService.QueueName, autoAck: false, consumer: consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var bodyArray = @event.Body.ToArray();
                var bodyString = Encoding.UTF8.GetString(bodyArray);
                var customer = JsonSerializer.Deserialize<Customer>(bodyString);

                //Pdf dönüştür ve e-mail gönder...

                _channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception e)
            {
                //Hatayı logla...
            }


            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
