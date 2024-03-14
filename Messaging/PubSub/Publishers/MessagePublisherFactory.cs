using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Messaging.PubSub.Publishers
{
    internal class MessagePublisherFactory : IMessagePublisherFactory
    {
        private IDictionary<string, IMessagePublisher> MessagePublishers { get; }
        private readonly IMqttClientFactory _mqttClientFactory;

        public IMessagePublisher GetOrCreateMessagePublisher(string hostName, int port)
        {
            var messagePublisherKey = $"{hostName.ToLower()}";
            if (!MessagePublishers.ContainsKey(messagePublisherKey))
                MessagePublishers.Add(messagePublisherKey, new MessagePublisher(hostName, port, _mqttClientFactory));

            return MessagePublishers[messagePublisherKey];
        }
        
        public MessagePublisherFactory(IMqttClientFactory mqttClientFactory)
        {
            _mqttClientFactory = mqttClientFactory;
            MessagePublishers = new ConcurrentDictionary<string, IMessagePublisher>();
        }

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            foreach (var messagePublisher in MessagePublishers)
            {
                messagePublisher.Value.Dispose();
            }

            MessagePublishers.Clear();
        }
    }
}
