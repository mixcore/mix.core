using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Mix.Queue.Models.QueueSetting;
using RabbitMQ.Client;

namespace Mix.Queue.Engines.RabbitMQ
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IChannel>
    {
        private readonly RabitMqQueueSetting _options;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(IOptions<RabitMqQueueSetting> optionsAccs)
        {
            _options = optionsAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
            };

            if (_options.Port.HasValue)
            {
                factory.Port = _options.Port.Value;
            }

            if (!string.IsNullOrEmpty(_options.UserName) && !string.IsNullOrEmpty(_options.Password))
            {
                factory.UserName = _options.UserName;
                factory.Password = _options.Password;
            }
            
            if (!string.IsNullOrEmpty(_options.VHost))
            {
                factory.VirtualHost = _options.VHost;
            }
            return factory.CreateConnectionAsync().Result;
        }

        public IChannel Create()
        {
            return _connection.CreateChannelAsync().Result;
        }

        public bool Return(IChannel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}