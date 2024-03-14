using Kernel.Messages;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using System.Text.Json;

namespace Messaging.PubSub.Subscribers
{
    internal class MessageSubscriber : IMessageSubscriber
    {
        //public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private readonly string _hostName;
        private readonly int _port;
        private readonly string _channelName;
        //private readonly ILogger<MessageSubscriber> _logger;
        private readonly IMqttClientFactory _mqttClientFactory;
        private readonly IEnumerable<IBaseMessage> _availableMessagesContracts;
        private IManagedMqttClient _client;

        public MessageSubscriber(string hostName, int port, string channel, IMqttClientFactory mqttClientFactory, IEnumerable<IBaseMessage> availableMessagesContracts)
        {
            _hostName = hostName;
            _port = port;
            _channelName = channel;
            _mqttClientFactory = mqttClientFactory;
            _availableMessagesContracts = availableMessagesContracts;
        }

        public async Task StartAsync()
        {
            _client ??= await EnsureClientAsync();
            try
            {
                _client.ApplicationMessageReceivedAsync += NewMessageReceived();
                await _client.SubscribeAsync(_channelName, MqttQualityOfServiceLevel.AtMostOnce);
                //_logger.LogBootstrap($"Subscriber connection established to '{_hostName}' on channel '{_channelName}'");
            }
            catch (Exception e)
            {
                //_logger.LogBootstrapError(
                //    $"Unexpected error while starting connection on '{_hostName}' for channel '{_channelName}'",
                //    e);
                throw;
            }
        }

        private Func<MqttApplicationMessageReceivedEventArgs, Task> NewMessageReceived()
        {
            return e =>
            {
                var message = e;

                //var utf8Reader = new Utf8JsonReader(e.ApplicationMessage.PayloadSegment);

                var baseMessage = JsonSerializer.Deserialize<BaseMessage>(e.ApplicationMessage.PayloadSegment);

                IBaseMessage availableContract = _availableMessagesContracts.SingleOrDefault(x => x.Type.Equals(baseMessage.Type));
                 
                if (availableContract != default)
                {
                    var test = availableContract.GetType();
                    var x = JsonSerializer.Deserialize(e.ApplicationMessage.PayloadSegment, test);
                }

                // Send do Message po MediaTR 


                // Dispatcher
                // Entrego 


                // Handler -|\
                // TODO Add behaviour here to notified messages
                //IQueueMessage message = null;
                //try
                //{
                //    message = (IQueueMessage)FromByteArray(e.ApplicationMessage.Payload); // TODO Alterar isto?
                //}
                //catch (Exception exception)
                //{
                //    _logger.LogServiceWarn(
                //        $"Failed to deserialize message on '{_hostName}' for channel '{e.ApplicationMessage.Topic}'\n",
                //        nameof(MessageSubscriber), exception);
                //}

                //MessageReceived?.Invoke(this, new MessageReceivedEventArgs($"{_hostName}_{e.ApplicationMessage.Topic}", message));
                //LogReceived(message);
                return Task.CompletedTask;
            };
        }

        public async Task StopAsync()
        {
            await _client.UnsubscribeAsync(_channelName);
            //_logger.LogBootstrap("Subscriber socket Disconnected and Closed");
        }

        private async Task<IManagedMqttClient> EnsureClientAsync()
        {
            //Mqtt is supposed to manage connections and channels auto recovery
            try
            {
                return await _mqttClientFactory.GetOrCreateAsync(_hostName, _port);
            }
            catch (Exception e)
            {
                //_logger.LogServiceWarn($"Unexpected error while creating connection for '{_hostName}'", nameof(MessageSubscriber), e);
                throw;
            }
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            StopAsync().Wait();
            //_logger.LogBootstrap($"Disposed subscriber on '{_hostName}' for channel {_channelName}");
        }
    }
}
