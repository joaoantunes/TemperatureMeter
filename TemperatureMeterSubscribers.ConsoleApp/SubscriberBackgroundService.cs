using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TemperatureMeterSubscribers.ConsoleApp
{
    public class SubscriberBackgroundService : BackgroundService
    {
        private readonly IMessageSubscriberFactory _messageSubscriberFactory;

        public SubscriberBackgroundService(ILogger<SubscriberBackgroundService> logger, IMessageSubscriberFactory messageSubscriberFactory)
        {
            _messageSubscriberFactory = messageSubscriberFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _messageSubscriberFactory.GetOrCreateMessageSubscriber("localhost", 1883, "iot");
            await subscriber.StartAsync();
        }

        //TODO call the dispose when terminated??
    }
}
