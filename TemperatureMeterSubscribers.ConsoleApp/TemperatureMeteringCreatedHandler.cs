using MediatR;
using Microsoft.Extensions.Logging;
using TemperatureMeter.Domain.Events;

namespace TemperatureMeterSubscribers.ConsoleApp
{
    public class TemperatureMeteringCreatedHandler : INotificationHandler<TemperatureMeteringCreated>
    {
        private readonly ILogger<TemperatureMeteringCreatedHandler> _logger;

        public TemperatureMeteringCreatedHandler(ILogger<TemperatureMeteringCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TemperatureMeteringCreated notification, CancellationToken cancellationToken)
        {
            ShowNewMetering(notification);
            return Task.CompletedTask;
        }

        private void ShowNewMetering(TemperatureMeteringCreated notification)
        {
            _logger.LogInformation($"NEW Reading => DeviceId: {notification.DeviceId} Temperature: {notification.TemperatureInCelcius}");
        }
    }
}
