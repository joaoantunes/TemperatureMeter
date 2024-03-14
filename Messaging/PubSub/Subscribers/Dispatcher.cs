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

        public Task DispatchAsync(MqttApplicationMessage newMessage)
        {
            var baseMessage = JsonSerializer.Deserialize<BaseNotification>(newMessage.PayloadSegment);

            IBaseNotification availableContract = _availableMessagesContracts.SingleOrDefault(x => x.Type.Equals(baseMessage.Type));

            if (availableContract == default)
            {
                throw new TypeLoadException($"Missing contract message for {baseMessage.Type}. Please verify that AddMessagesContracts were added on the right Assembly");           
            }

            var messageType = availableContract.GetType();
            var newNotification = JsonSerializer.Deserialize(newMessage.PayloadSegment, messageType);
            return _mediator.Publish(newNotification);
        }
    }
}
