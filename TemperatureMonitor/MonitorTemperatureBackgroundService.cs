using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TemperatureMonitor.ConsoleApp
{
    public class MonitorTemperatureBackgroundService : BackgroundService
    {
        private readonly ILogger<MonitorTemperatureBackgroundService> _logger;
        private readonly IMessageSubscriberFactory _messageSubscriberFactory;

        public MonitorTemperatureBackgroundService(ILogger<MonitorTemperatureBackgroundService> logger, IMessageSubscriberFactory messageSubscriberFactory)
        {
            _logger = logger;
            _messageSubscriberFactory = messageSubscriberFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _messageSubscriberFactory.GetOrCreateMessageSubscriber("localhost", 1883, "iot"); 
            await subscriber.StartAsync();
        }
    }
}
