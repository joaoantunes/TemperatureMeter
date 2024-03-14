using System.Collections.Concurrent;

namespace Messaging.PubSub.Subscribers
{
    internal class MessageSubscriberFactory : IMessageSubscriberFactory
    {
        private bool _disposed;
        private IDictionary<string, IMessageSubscriber> MessageSubscribers { get; } = new ConcurrentDictionary<string, IMessageSubscriber>();
        private readonly Func<string, int, string, MessageSubscriber> _messageSubscriberFunc;

        public IMessageSubscriber GetOrCreateMessageSubscriber(string hostName, int port, string channel) 
        {
            var key = $"{hostName.ToLower()}_{port}_{channel.ToLower()}";

            if (!MessageSubscribers.ContainsKey(key))
                MessageSubscribers.Add(key, _messageSubscriberFunc(hostName, port, channel));

            return MessageSubscribers[key];
        }

        public MessageSubscriberFactory(Func<string, int, string, MessageSubscriber> messageSubscriberFunc)
        {
             _messageSubscriberFunc = messageSubscriberFunc;
        }


        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            foreach (var messagePublisher in MessageSubscribers)
            {
                messagePublisher.Value.Dispose();
            }

            MessageSubscribers.Clear();
        }

    }
}
