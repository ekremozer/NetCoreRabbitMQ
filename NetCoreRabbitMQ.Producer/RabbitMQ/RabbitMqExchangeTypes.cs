using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace NetCoreRabbitMQ.Producer.RabbitMQ
{
    public static class RabbitMqExchangeTypes
    {
        public static void FanoutExchange()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "FanoutExchange-example", type: ExchangeType.Fanout, durable: true);

            Enumerable.Range(0, 60).ToList().ForEach(x =>
            {
                var message = $"ekremozer.com | FanoutExchange: {x}";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "FanoutExchange-example", routingKey: string.Empty, basicProperties: null, body: messageBytes);
                Console.WriteLine(message);
            });
            Console.ReadLine();
        }

        public static void DirectExchange()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "DirectExchange-example", type: ExchangeType.Direct, durable: true);

            channel.QueueDeclare(queue: "direct-queue-admin", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: "direct-queue-admin", exchange: "DirectExchange-example", routingKey: "direct-route-admin");

            channel.QueueDeclare(queue: "direct-queue-moderator", durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: "direct-queue-moderator", exchange: "DirectExchange-example", routingKey: "direct-route-moderator");

            Enumerable.Range(0, 15).ToList().ForEach(x =>
            {
                var message = $"ekremozer.com | DirectExchange Admin: {x}";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "DirectExchange-example", routingKey: "direct-route-admin", basicProperties: null, body: messageBytes);
                Console.WriteLine(message);
            });

            Enumerable.Range(0, 15).ToList().ForEach(x =>
            {
                var message = $"ekremozer.com | DirectExchange Moderator: {x}";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "DirectExchange-example", routingKey: "direct-route-moderator", basicProperties: null, body: messageBytes);
                Console.WriteLine(message);
            });

            Console.ReadLine();
        }

        public static void TopicExchange()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "TopicExchange-example", type: ExchangeType.Topic, durable: true);

            var routeKeys = new[] { "Admin.Message.High", "Mod.Message.High", "Admin.Message.Low", "Mod.Message.Low" };

            routeKeys.ToList().ForEach(routeKey =>
            {
                Enumerable.Range(1, 6).ToList().ForEach(x =>
                {
                    var message = $"ekremozer.com | TopicExchange | {routeKey}: {x}";
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "TopicExchange-example", routingKey: routeKey, basicProperties: null, body: messageBytes);
                    Console.WriteLine(message);
                });

            });

            Console.ReadLine();
        }

        public static void HeaderExchange()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "HeaderExchange-example", type: ExchangeType.Topic, durable: true);

            var headers = new Dictionary<string, object>
            {
                { "Role", "Admin" },
                { "Level", "High" }
            };

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;
            properties.Persistent = true;

            var message = "ekremozer.com | HeaderExchange";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "HeaderExchange-example", routingKey: string.Empty, basicProperties: properties, body: messageBytes);

            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static void WithoutExchange()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "hello-world", durable: true, exclusive: false, autoDelete: false);
                Enumerable.Range(0, 60).ToList().ForEach(x =>
                {
                    var message = $"ekremozer.com | mesaj: {x}";
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: string.Empty, routingKey: "hello-world", basicProperties: null, body: messageBytes);
                    Console.WriteLine(message);
                });
            }
            Console.ReadLine();
        }
    }
}
