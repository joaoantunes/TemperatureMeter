using MediatR;
using Messaging.PubSub.Publishers;
using TemperatureMeter.Domain.Converter;
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

        public CreateTemperatureReadingCommandHandler(IMessagePublisherFactory messagePublisherFactory, IKelvinToCelsiusConverter kelvinToCelsiusConverter)
        {
            _messagePublisherFactory = messagePublisherFactory;
            _kelvinToCelsiusConverter = kelvinToCelsiusConverter;
        }

        public async Task<bool> Handle(CreateTemperatureReadingCommand request, CancellationToken cancellationToken)
        {
            var temperatureEventMessage = new TemperatureMeteringCreated() { TemperatureInCelcius = _kelvinToCelsiusConverter.Convert(request.TemperatureInKelvin) };
            var publisher = _messagePublisherFactory.GetOrCreateMessagePublisher("localhost", 1883); // TODO add configs
            var result = await publisher.PublishAsync(temperatureEventMessage, "iot");

            // Could store the value of Metering on a DB

            return result;
        }
    }
}
