using System;
using Newtonsoft.Json;
using RabbitMq.Domain.Event;
using RabbitMq.Domain.Model;

namespace RabbitMq.Consumer.Consumers
{
    public class WalletCreatedConsumer : IConsumerHandler<EventData>
    {
        public bool Handle(EventData eventData)
        {
            var data = JsonConvert.DeserializeObject<WalletCreatedEvent>(eventData.Data.ToString());
            Console.WriteLine($"get event data {data.Id}");
            return true;
        }
    }
}