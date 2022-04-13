using System;
using Autofac;
using RabbitMQ.Client;
using RabbitMq.Consumer.Applibs;
using RabbitMq.Domain.Model;

namespace RabbitMq.Consumer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || !int.TryParse(args[0], out int queueId))
            {
                throw new Exception("get queue fail");
            }

            var connFactory = AutofacConfig.Container.Resolve<RabbitMqFactory>();
            connFactory.Connect();
            var dispatcher = AutofacConfig.Container.Resolve<EventDispatcher>();
            connFactory.NewConsumer(dispatcher, "WalletService", queueId.ToString(), ExchangeType.Direct);
            while (Console.ReadLine().ToLower() != "exit")
            {
            }
        }
    }
}