using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TemperatureMeterSubscribers.ConsoleApp
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly IMessageSubscriberFactory _messageSubscriberFactory;

        public MyBackgroundService(ILogger<MyBackgroundService> logger, IMessageSubscriberFactory messageSubscriberFactory)
        {
            _logger = logger;
            _messageSubscriberFactory = messageSubscriberFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _messageSubscriberFactory.GetOrCreateMessageSubscriber("localhost", 1883, "iot"); // TODO Probably can change the channel to Start instead of creation
            await subscriber.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                
            }
        }

    }
}
