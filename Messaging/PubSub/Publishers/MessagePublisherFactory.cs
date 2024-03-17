using System.Collections.Concurrent;

namespace Messaging.PubSub.Publishers
{
    internal class MessagePublisherFactory : IMessagePublisherFactory
    {
        private IDictionary<string, IMessagePublisher> MessagePublishers { get; }
        private readonly IMqttClientFactory _mqttClientFactory;
        private readonly Func<string, int, MessagePublisher> _messagePublisherFunc;

        public IMessagePublisher GetOrCreateMessagePublisher(string hostName, int port)
        {
            var messagePublisherKey = $"{hostName.ToLower()}";
            if (!MessagePublishers.ContainsKey(messagePublisherKey))
                MessagePublishers.Add(messagePublisherKey, _messagePublisherFunc(hostName, port));

            return MessagePublishers[messagePublisherKey];
        }
        
        public MessagePublisherFactory(IMqttClientFactory mqttClientFactory, Func<string, int, MessagePublisher> messagePublisherFunc)
        {
            _mqttClientFactory = mqttClientFactory;
            _messagePublisherFunc = messagePublisherFunc;
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
