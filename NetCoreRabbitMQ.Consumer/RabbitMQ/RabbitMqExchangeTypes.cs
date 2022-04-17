using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetCoreRabbitMQ.Consumer.RabbitMQ
{
    public static class RabbitMqExchangeTypes
    {
        public static void FanoutExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: randomQueueName, exchange: "FanoutExchange-example", routingKey: string.Empty);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: randomQueueName, autoAck: false, consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            Console.ReadLine();
        }

        public static void DirectExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: "direct-queue-admin", autoAck: false, consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            Console.ReadLine();
        }

        public static void TopicExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);

            var randomQueueName = channel.QueueDeclare().QueueName;
            var routeKey = "#.High";
            channel.QueueBind(queue: randomQueueName, exchange: "TopicExchange-example", routingKey: routeKey);

            channel.BasicConsume(queue: randomQueueName, autoAck: false, consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            Console.ReadLine();
        }

        public static void HeaderExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);

            var randomQueueName = channel.QueueDeclare().QueueName;
            var headers = new Dictionary<string, object>
            {
                { "Role", "Admin" },
                { "Level", "High" },
                { "x-match", "all" }
            };

            channel.QueueBind(queue: randomQueueName, exchange: "HeaderExchange-example", routingKey: string.Empty, arguments: headers);

            channel.BasicConsume(queue: randomQueueName, autoAck: false, consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            Console.ReadLine();
        }

        public static void WithoutExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://fqykbkan:fiHTx9mjpDk33teVSASdjex-dk-vlbTe@fox.rmq.cloudamqp.com/fqykbkan")
            };

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello-world", durable: true, exclusive: false, autoDelete: false);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: "hello-world", autoAck: false, consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine(message);

                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            Console.ReadLine();
        }
    }
}
