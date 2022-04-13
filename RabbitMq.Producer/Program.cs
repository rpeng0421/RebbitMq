using System;
using Autofac;
using RabbitMq.Domain.Model;
using RabbitMq.PubSubData.Applibs;

namespace RabbitMq.PubSubData
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var connFactory = AutofacConfig.Container.Resolve<RabbitMqFactory>();
            connFactory.Connect();
            var createWalletsApp = AutofacConfig.Container.Resolve<CreateWallets>();
            createWalletsApp.Execute();
        }
    }
}