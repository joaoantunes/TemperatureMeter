namespace Messaging.PubSub.Subscribers
{
    public interface IMessageSubscriberFactory : IDisposable
    {

        IMessageSubscriber GetOrCreateMessageSubscriber(string hostName, int port, string channel); 
    }
}
