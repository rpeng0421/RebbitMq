using System.Configuration;

namespace RabbitMq.PubSubData.Applibs
{
    public class ConfigHelper
    {
        public static readonly string RmqExpiration = ConfigurationManager.AppSettings["RmqExpiration"].ToString();
        public static readonly string RabbitMqUri = ConfigurationManager.AppSettings["RabbitMqUri"].ToString();
    }
}