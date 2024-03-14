using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace Messaging.PubSub.Subscribers
{
    internal class MessageSubscriber : IMessageSubscriber
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _channelName;
        private readonly IMqttClientFactory _mqttClientFactory;
        private readonly IDispatcher _dispatcher;
        private IManagedMqttClient _client;

        public MessageSubscriber(string hostName, int port, string channel, IMqttClientFactory mqttClientFactory, IDispatcher dispatcher)
        {
            _hostName = hostName;
            _port = port;
            _channelName = channel;
            _mqttClientFactory = mqttClientFactory;
            _dispatcher = dispatcher;
        }

        public async Task StartAsync()
        {
            _client ??= await EnsureClientAsync();
            _client.ApplicationMessageReceivedAsync += NewMessageReceived();
            await _client.SubscribeAsync(_channelName, MqttQualityOfServiceLevel.AtMostOnce);
        }

        private Func<MqttApplicationMessageReceivedEventArgs, Task> NewMessageReceived()
        {
            return e =>
            {
                _dispatcher.DispatchAsync(e.ApplicationMessage);
                return Task.CompletedTask;
            };
        }

        public async Task StopAsync()
        {
            await _client.UnsubscribeAsync(_channelName);
        }

        private async Task<IManagedMqttClient> EnsureClientAsync()
        {
            //Mqtt is supposed to manage connections and channels auto recovery
            return await _mqttClientFactory.GetOrCreateAsync(_hostName, _port);
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            StopAsync().Wait();
        }
    }
}
