using MicroServices.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MicroServices.Infrastructure.MessageBroker
{
    public class RabbitMQClient
    {
        private readonly IOptions<RabbitMQSettings> _options;
        private readonly ConnectionFactory _factory;
        
        public RabbitMQClient(IOptions<RabbitMQSettings> options)
        {
            _options = options;
            _factory = new ConnectionFactory()
            {
                HostName = _options.Value.Host,
                Port = _options.Value.Port,
                UserName = _options.Value.User,
                Password = _options.Value.Password
            };
        }

        public ConnectionFactory Factory()
        {
            return _factory;
        }
    }
}