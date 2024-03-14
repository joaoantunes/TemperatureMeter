using MQTTnet;

namespace Messaging.PubSub.Subscribers
{
    internal interface IDispatcher
    {
        Task DispatchAsync(MqttApplicationMessage newMessage);
    }
}
