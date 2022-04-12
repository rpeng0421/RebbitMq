using System.Reflection.Emit;

namespace RabbitMq.Domain.Model
{
    public class EventData
    {
        public string Type { get; set; }
        public long Timestamp { get; set; }
        public string Data { get; set; }
    }
}