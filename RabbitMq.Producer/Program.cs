using System;
using Autofac;
using RabbitMq.PubSubData.Applibs;

namespace RabbitMq.PubSubData
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (!int.TryParse(args[0], out int queueId))
            {
                throw new Exception("get queue fail");
            }

            var createWalletsApp = AutofacConfig.Container.Resolve<CreateWallets>();
            createWalletsApp.Execute();
        }
    }
}