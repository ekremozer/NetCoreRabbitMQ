using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetCoreRabbitMQ.Web.Models;
using RabbitMQ.Client;

namespace NetCoreRabbitMQ.Web.Services
{
    public class RabbitMQProducer
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQProducer(RabbitMQClientService rabbitMqClientService)
        {
            _rabbitMQClientService = rabbitMqClientService;
        }

        public void Publish(Customer customer)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyJson = JsonSerializer.Serialize(customer);

            var bodyByte = Encoding.UTF8.GetBytes(bodyJson);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingInvoice, basicProperties: properties, body: bodyByte);
        }
    }
}
