using Kernel.Messages;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Text.Json;

namespace Messaging.PubSub.Publishers
{
    internal class MessagePublisher : IMessagePublisher
    {
        private readonly Lazy<IManagedMqttClient> LazyClient;

        public MessagePublisher(string hostName, int port, IMqttClientFactory clientFactory)
        {
            LazyClient = new Lazy<IManagedMqttClient>(() => clientFactory.GetOrCreateAsync(hostName, port).GetAwaiter().GetResult());
        }

        public async Task<bool> PublishAsync<T>(T payload, string topic) where T : BaseNotification
        {
            var payloadBytes = JsonSerializer.SerializeToUtf8Bytes(payload);

            var message = new MqttApplicationMessageBuilder()
              .WithTopic(topic)
              .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
              .WithPayload(payloadBytes)
              .Build();

            await LazyClient.Value.EnqueueAsync(message);
            return true;
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }
    }
}
