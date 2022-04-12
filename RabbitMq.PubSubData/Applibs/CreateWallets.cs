using RabbitMq.Domain.Model;

namespace RabbitMq.PubSubData.Applibs
{
    public class CreateWallets
    {
        private RabbitMqFactory rabbitMqFactory;

        public CreateWallets(RabbitMqFactory rabbitMqFactory)
        {
            this.rabbitMqFactory = rabbitMqFactory;
        }

        public void Execute()
        {
            
        }
    }
}