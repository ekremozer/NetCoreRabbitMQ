using System;
using System.Linq;
using System.Text;
using NetCoreRabbitMQ.Producer.RabbitMQ;
using RabbitMQ.Client;

namespace NetCoreRabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqExchangeTypes.HeaderExchange();
        }
    }
}
