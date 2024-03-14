using Kernel.Messages;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Text.Json;

namespace Messaging.PubSub.Publishers
{
    internal class MessagePublisher : IMessagePublisher
    {
        private readonly IMqttClientFactory _clientFactory;
        private readonly string _hostName;
        private readonly int _port;
        private IManagedMqttClient? _client;

        public MessagePublisher(string hostName, int port, IMqttClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _hostName = hostName;
            _port = port;
        }

        private async Task EnsureClientAsync()
        {
            //Mqtt Managed Client is supposed to manage connections and channels auto recovery
            _client ??= await _clientFactory.GetOrCreateAsync(_hostName, _port);
        }

        public async Task<bool> PublishAsync<T>(T payload, string topic) where T : BaseNotification
        {
            await EnsureClientAsync();
            var payloadBytes = JsonSerializer.SerializeToUtf8Bytes(payload); //TODO Add ISerializer

            var message = new MqttApplicationMessageBuilder()
              .WithTopic(topic)
              .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
              .WithPayload(payloadBytes)
              .Build();

            await _client.EnqueueAsync(message);
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
