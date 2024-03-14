using MQTTnet.Extensions.ManagedClient;

namespace Messaging.PubSub
{
    internal interface IMqttClientFactory
    {
        Task<IManagedMqttClient> GetOrCreateAsync(string hostName, int port);
    }
}