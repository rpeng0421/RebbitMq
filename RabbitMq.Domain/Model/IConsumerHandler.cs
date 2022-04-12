﻿namespace RabbitMq.Domain.Model
{
    public interface IConsumerHandler<TEvent> where TEvent : EventData
    {
        bool Handle(EventData eventData);
    }
}