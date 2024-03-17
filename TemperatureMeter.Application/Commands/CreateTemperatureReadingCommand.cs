using MediatR;
using Messaging.PubSub.Publishers;
using Microsoft.Extensions.Logging;
using TemperatureMeter.Domain.Converters;
using TemperatureMeter.Domain.Events;

namespace TemperatureMeter.Application.Commands
{
    public  class CreateTemperatureReadingCommand : IRequest<bool>
    {
        public Guid DeviceId { get; set; }
        public double TemperatureInKelvin { get; set; }
    }

    public class CreateTemperatureReadingCommandHandler : IRequestHandler<CreateTemperatureReadingCommand, bool>
    {
        private readonly IMessagePublisherFactory _messagePublisherFactory;
        private readonly IKelvinToCelsiusConverter _kelvinToCelsiusConverter;
        private readonly ILogger<CreateTemperatureReadingCommandHandler> _logger;

        public CreateTemperatureReadingCommandHandler(IMessagePublisherFactory messagePublisherFactory, IKelvinToCelsiusConverter kelvinToCelsiusConverter, 
            ILogger<CreateTemperatureReadingCommandHandler> logger)
        {
            _messagePublisherFactory = messagePublisherFactory;
            _kelvinToCelsiusConverter = kelvinToCelsiusConverter;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateTemperatureReadingCommand request, CancellationToken cancellationToken)
        {
            // Could store the value of Metering on a DB before publishing the event

            bool result = false;
            TemperatureMeteringCreated temperatureEventMessage;
            try
            {
                temperatureEventMessage = new TemperatureMeteringCreated() 
                {
                    DeviceId = request.DeviceId,
                    TemperatureInCelcius = _kelvinToCelsiusConverter.Convert(request.TemperatureInKelvin)
                };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Not able to process the CreateTemperatureReadingCommand");
                return result;
            }

            try
            {
                var publisher = _messagePublisherFactory.GetOrCreateMessagePublisher("localhost", 1883); // TODO add configs
                result = await publisher.PublishAsync(temperatureEventMessage, "iot"); // TODO add consts
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to publish TemperatureMeteringCreated");
                return result;
            }

            //

            return result;
        }
    }
}
