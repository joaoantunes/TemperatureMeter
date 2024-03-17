using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace Messaging.PubSub.Subscribers
{
    internal class MessageSubscriber : IMessageSubscriber
    {
        private readonly string _channelName;
        private readonly IMqttClientFactory _mqttClientFactory;
        private readonly IDispatcher _dispatcher;
        private readonly Lazy<IManagedMqttClient> LazyClient;

        public MessageSubscriber(string hostName, int port, string channel, IMqttClientFactory mqttClientFactory, IDispatcher dispatcher)
        {
            _channelName = channel;
            _mqttClientFactory = mqttClientFactory;
            _dispatcher = dispatcher;
            LazyClient = new Lazy<IManagedMqttClient>(() => _mqttClientFactory.GetOrCreateAsync(hostName, port).GetAwaiter().GetResult());
        }

        public async Task StartAsync()
        {
            LazyClient.Value.ApplicationMessageReceivedAsync += NewMessageReceived();
            await LazyClient.Value.SubscribeAsync(_channelName, MqttQualityOfServiceLevel.AtMostOnce);
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
            await LazyClient.Value.UnsubscribeAsync(_channelName);
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
