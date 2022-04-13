using System;
using RabbitMq.Domain.Helper;
using RabbitMq.Domain.Model;

namespace RabbitMq.Domain.Event
{
    public class WalletCreatedEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}