using System.Reflection.Emit;
using Newtonsoft.Json;

namespace RabbitMq.Domain.Model
{
    public class EventData
    {
        public string Type { get; set; }
        public long Timestamp { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}