using System;
using Autofac.Features.Indexed;
using Newtonsoft.Json;
using RabbitMq.Domain.Model;

namespace RabbitMq.Consumer.Applibs
{
    public class EventDispatcher : IConsumerHandler<EventData>
    {
        private IIndex<string, IConsumerHandler<EventData>> consumerSet;

        public EventDispatcher(IIndex<string, IConsumerHandler<EventData>> consumerSet)
        {
            this.consumerSet = consumerSet;
        }

        public bool Handle(EventData eventData)
        {
            Console.WriteLine($"get event data {JsonConvert.SerializeObject(eventData)}");
            var handlerName = eventData.Type.Replace("Event", "");
            if (this.consumerSet.TryGetValue(handlerName, out var handler))
            {
                handler.Handle(eventData);
                return true;
            }

            return false;
        }
    }
}