using System.Text;
using MicroServices.Core.Event;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MicroServices.Infrastructure.MessageBroker
{
    public abstract class EventPublisher<TEvent> : IPublisher<TEvent> where TEvent : EventMessage
    {
        protected string Queue { get; init; }
        private readonly RabbitMQClient _client;

        protected EventPublisher(RabbitMQClient client)
        {
            _client = client;
        }

        public void Publish(TEvent @event)
        {
            using IConnection connection = _client.Factory().CreateConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            channel.BasicPublish(exchange: "",
                routingKey: Queue,
                basicProperties: null,
                body: body);
        }
    }
}