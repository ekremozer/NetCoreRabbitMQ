using System;
using System.Text;
using NetCoreRabbitMQ.Consumer.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace NetCoreRabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqExchangeTypes.HeaderExchange();
        }
    }
}
