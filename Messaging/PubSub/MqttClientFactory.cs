using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;

namespace Messaging.PubSub
{
    internal class MqttClientFactory : IMqttClientFactory, IDisposable
    {
        private readonly SemaphoreSlim _clientSemaphore = new(1);
        private readonly IDictionary<string, IManagedMqttClient> _clients = new Dictionary<string, IManagedMqttClient>();

        public async Task<IManagedMqttClient> GetOrCreateAsync(string hostName, int port)
        {
            await _clientSemaphore.WaitAsync();
            try
            {
                string endpoint = hostName + port;
                if (_clients.TryGetValue(endpoint, out IManagedMqttClient? value))
                    return value;

                var client = await CreateClientAsync(hostName, port);
                _clients.Add(endpoint, client);

                return client;
            }
            finally
            {
                _clientSemaphore.Release();
            }
        }

        private async Task<IManagedMqttClient> CreateClientAsync(string hostName, int port)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithTcpServer(hostName, port)
                    .Build())
                .Build();

            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            await mqttClient.StartAsync(options);

            return mqttClient;
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            _clients.Values.ToList().ForEach(c =>
            {
                c.Dispose();
            });
            _clients.Clear();
        }
    }
}
