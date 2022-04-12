using RabbitMq.Domain.Model;

namespace RabbitMq.Domain.Event
{
    public class WalletCreatedEvent : EventData
    {
        private string Id { get; set; }
        private string Name { get; set; }
    }
}