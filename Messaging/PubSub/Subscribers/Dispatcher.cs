using Kernel.Messages;
using MediatR;
using MQTTnet;
using System.Text.Json;

namespace Messaging.PubSub.Subscribers
{
    internal class Dispatcher : IDispatcher
    {
        private readonly IPublisher _mediator;
        private readonly IEnumerable<IBaseNotification> _availableMessagesContracts;

        public Dispatcher(IPublisher mediator, IEnumerable<IBaseNotification> availableMessagesContracts)
        {
            _mediator = mediator;
            _availableMessagesContracts = availableMessagesContracts;
        }

        public async Task DispatchAsync(MqttApplicationMessage newMessage)
        {
            var baseMessage = JsonSerializer.Deserialize<BaseNotification>(newMessage.PayloadSegment);
            if (baseMessage == default || baseMessage.TypeContract == typeof(BaseNotification).FullName)
            {
                throw new ArgumentException("Message to be dispatched MUST Inherit from BaseNotification Type.");
            }

            IBaseNotification? availableContract = _availableMessagesContracts.SingleOrDefault(x => x.TypeContract.Equals(baseMessage.TypeContract));
            if (availableContract == default)
            {
                throw new TypeLoadException($"Missing contract message for {baseMessage.TypeContract}. Please verify that AddMessagesContracts were added on the right Assembly");
            }

            var messageType = availableContract.GetType();
            var newNotification = JsonSerializer.Deserialize(newMessage.PayloadSegment, messageType)
                ?? throw new JsonException($"Unable to deserialize payload to messageType: '{messageType}'");

            await _mediator.Publish(newNotification);
        }
    }
}
