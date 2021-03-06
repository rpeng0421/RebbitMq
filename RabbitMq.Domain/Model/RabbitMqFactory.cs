using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Domain.Model
{
    public class RabbitMqFactory : IDisposable
    {
        private ConnectionFactory factory;

        private IConnection connection;

        public readonly string RabbitUrl;

        public readonly string RmqExpiration;

        private Dictionary<string, IModel> models = new Dictionary<string, IModel>();

        public RabbitMqFactory(string rabbitUrl, string rmqExpiration)
        {
            RabbitUrl = rabbitUrl;
            RmqExpiration = rmqExpiration;
            factory = new ConnectionFactory()
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                UserName = "admin",
                Password = "admin"
            };
        }

        public void NewConsumer(IConsumerHandler<EventData> handler, string topic, string queueId, string exchangeType)
        {
            var (exchangeKey, model) = this.GetModel(topic, exchangeType);
            var queueName = model.QueueDeclare($"{queueId}-{topic}", false, false, false, null).QueueName;
            model.QueueBind(queueName, exchangeKey, string.Empty, null);
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, args) =>
            {
                var @event = JsonConvert.DeserializeObject<EventData>(Encoding.UTF8.GetString(args.Body.ToArray()));
                var handleResult = handler.Handle(@event);
                if (handleResult)
                {
                    model.BasicAck(args.DeliveryTag, true);
                    return;
                }

                model.BasicNack(args.DeliveryTag, true, true);
            };
            var consumerTag = model.BasicConsume(queueName, false, $"{Environment.MachineName}", false, false, null,
                consumer);
        }

        public void PublishDirect<T>(string topicName, T data) where T : EventData
        {
            this.Publish(topicName, data, ExchangeType.Direct);
        }

        public void PublishFanout<T>(string topicName, T data) where T : EventData
        {
            this.Publish(topicName, data, ExchangeType.Fanout);
        }

        public void Publish<T>(string topicName, T data, string exchangeType) where T : EventData
        {
            var (exchangeKey, model) = this.GetModel(topicName, exchangeType);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var prop = model.CreateBasicProperties();
            prop.Expiration = RmqExpiration;

            model.BasicPublish(exchangeKey, string.Empty, prop, body);
        }

        public (string exchangeKey, IModel model) GetModel(string topic, string exchangeType)
        {
            var modelKey = this.GetModelKey(topic, exchangeType);
            var exchangeKey = $"Exchange-{exchangeType}-{topic}";
            if (!this.models.ContainsKey(modelKey))
            {
                var model = connection.CreateModel();
                model.ExchangeDeclare(exchangeKey, exchangeType);
                models.Add(modelKey, model);
            }

            return (exchangeKey, models[modelKey]);
        }

        /// <summary>
        /// key: "topic-exchangeType"
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        private string GetModelKey(string topic, string exchangeType)
        {
            return $"${topic}-{exchangeType}";
        }

        public void Connect()
        {
            connection = factory.CreateConnection(AmqpTcpEndpoint.ParseMultiple(RabbitUrl));
        }

        public void Stop()
        {
            foreach (var model in models)
            {
                model.Value.Abort();
                model.Value.Close();
            }

            models = new Dictionary<string, IModel>();
            connection.Abort();
            connection.Close();
        }

        public void Dispose()
        {
            foreach (var model in models)
            {
                model.Value.Abort();
                model.Value.Close();
            }

            connection?.Dispose();
        }
    }
}