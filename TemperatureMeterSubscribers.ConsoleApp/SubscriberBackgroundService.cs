using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TemperatureMeterSubscribers.ConsoleApp
{
    public class SubscriberBackgroundService : BackgroundService
    {
        private readonly ILogger<SubscriberBackgroundService> _logger;
        private readonly IMessageSubscriberFactory _messageSubscriberFactory;

        public SubscriberBackgroundService(ILogger<SubscriberBackgroundService> logger, IMessageSubscriberFactory messageSubscriberFactory)
        {
            _logger = logger;
            _messageSubscriberFactory = messageSubscriberFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _messageSubscriberFactory.GetOrCreateMessageSubscriber("localhost", 1883, "iot"); // TODO Probably can change the channel to Start instead of creation
            await subscriber.StartAsync();
        }

        //TODO call the dispose when terminated??
    }
}
