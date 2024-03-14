namespace Messaging.PubSub.Publishers
{
    public interface IMessagePublisherFactory : IDisposable
    {
        IMessagePublisher GetOrCreateMessagePublisher(string hostName, int port);
    }
}
