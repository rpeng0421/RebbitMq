using RabbitMq.Domain.Model;

namespace RabbitMq.Domain.Event
{
    public class WalletCreatedEvent : EventData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}