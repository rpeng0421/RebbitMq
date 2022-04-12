namespace RabbitMq.Domain.Model
{
    public interface IConsumer
    {
        string Topic { get; set; }
        string QueueId { get; set; }
        string ExchangeType { get; set; }
    }
}