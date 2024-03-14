using MediatR;
using Microsoft.Extensions.Logging;
using TemperatureMeter.Domain.Events;

namespace TemperatureMonitor.ConsoleApp
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
            if (notification.TemperatureInCelcius > 75)
            {
                _logger.LogCritical($"ALERT OVERHEATING DEVICE => DeviceId: {notification.DeviceId} Temperature: {notification.TemperatureInCelcius}");

            } else if (notification.TemperatureInCelcius < 0)
            {
                _logger.LogCritical($"ALERT FREEZING DEVICE => DeviceId: {notification.DeviceId} Temperature: {notification.TemperatureInCelcius}");
            }

            return Task.CompletedTask;
        }
    }
}
