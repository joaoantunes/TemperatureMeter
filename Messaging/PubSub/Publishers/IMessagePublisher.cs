
using Kernel.Messages;

namespace Messaging.PubSub.Publishers
{
    public interface IMessagePublisher : IDisposable
    {
        Task<bool> PublishAsync<T>(T payload, string topic) where T : BaseMessage;
    }
}
