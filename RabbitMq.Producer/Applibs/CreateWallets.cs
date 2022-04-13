using System;
using System.Linq;
using System.Threading;
using RabbitMq.Domain.Event;
using RabbitMq.Domain.Helper;
using RabbitMq.Domain.Model;

namespace RabbitMq.PubSubData.Applibs
{
    public class CreateWallets
    {
        private RabbitMqFactory rabbitMqFactory;

        public CreateWallets(RabbitMqFactory rabbitMqFactory)
        {
            this.rabbitMqFactory = rabbitMqFactory;
        }

        public void Execute()
        {
            var index = 1;
            while (true)
            {
                this.rabbitMqFactory.PublishDirect("WalletService", new WalletCreatedEvent
                {
                    Type = nameof(WalletCreatedEvent),
                    Timestamp = TimestampHelper.Now(),
                    Id = $"walletId-{index}",
                    Name = $"walletName-{index}"
                });
                index++;
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}