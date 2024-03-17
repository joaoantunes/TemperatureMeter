using Kernel.Messages;
using MediatR;
using Messaging.PubSub.Subscribers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MQTTnet;
using NSubstitute;
using System.Text.Json;
using Xunit;

namespace Messaging.Tests.PubSub.Subscribers
{
    public class DispatcherTests
    {
        public class TestBaseNotification : BaseNotification { }

        [Fact] 
        public async Task DispatchAsync_GivenMessageDoesNotInheritFromBaseNotification_ThrowsException()
        {
            // Arrange 
            var wrongMessageType = new Message();
            var applicationMessage = new MqttApplicationMessage() { PayloadSegment = JsonSerializer.SerializeToUtf8Bytes(wrongMessageType) };
            var expectedException = new ArgumentException($"Message to be dispatched MUST Inherit from BaseNotification Type.");

            var publisherMediatorMock = Substitute.For<IPublisher>();
            var emptyContractsMock = new List<IBaseNotification>() { };

            // Act
            var sut = new Dispatcher(publisherMediatorMock, emptyContractsMock);
            Task act() => sut.DispatchAsync(applicationMessage);

            // Assert
            var resultException = await Assert.ThrowsAsync<ArgumentException>(act);
            Assert.Equal(expectedException.Message, resultException.Message);
        }

        [Fact]
        public async Task DispatchAsync_GivenMessageWithoutRegisteredContract_ThrowsTypeLoadException()
        {
            // Arrange 
            var notificationToDispatch = new TestBaseNotification();
            var applicationMessage = new MqttApplicationMessage() { PayloadSegment = JsonSerializer.SerializeToUtf8Bytes(notificationToDispatch) };
            var expectedException = new TypeLoadException($"Missing contract message for {notificationToDispatch.TypeContract}. Please verify that AddMessagesContracts were added on the right Assembly");

            var publisherMediatorMock = Substitute.For<IPublisher>();
            var emptyContractsMock = new List<IBaseNotification>() { };

            // Act
            var sut = new Dispatcher(publisherMediatorMock, emptyContractsMock);
            Task act() => sut.DispatchAsync(applicationMessage);

            // Assert
            var resultException = await Assert.ThrowsAsync<TypeLoadException>(act);
            Assert.Equal(expectedException.Message, resultException.Message);
        }

        [Fact]
        public async Task DispatchAsync_GivenDuplicatedRegisteredMessageContracts_ThrowsInvalidOperationException()
        {
            // Arrange 
            var notificationToDispatch = new TestBaseNotification();
            var applicationMessage = new MqttApplicationMessage() { PayloadSegment = JsonSerializer.SerializeToUtf8Bytes(notificationToDispatch) };
            var expectedException = new InvalidOperationException("Sequence contains more than one matching element");

            var publisherMediatorMock = Substitute.For<IPublisher>();
            var duplicatedContractsRegisteredContractsMock = new List<IBaseNotification>() { notificationToDispatch, notificationToDispatch };

            // Act
            var sut = new Dispatcher(publisherMediatorMock, duplicatedContractsRegisteredContractsMock);
            Task act() => sut.DispatchAsync(applicationMessage);

            // Assert
            var resultException = await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Equal(expectedException.Message, resultException.Message);
        }

        [Fact]
        public async Task DispatchAsync_GivenMessageWithRightRegistration_DispatchesMessage()
        {
            // Arrange 
            var notificationToDispatch = new TestBaseNotification();
            var applicationMessage = new MqttApplicationMessage() { PayloadSegment = JsonSerializer.SerializeToUtf8Bytes(notificationToDispatch) };
            var publisherMediatorMock = Substitute.For<IPublisher>();
            var availableContractsMock = new List<IBaseNotification>() { notificationToDispatch };

            // Act
            var sut = new Dispatcher(publisherMediatorMock, availableContractsMock);
            await sut.DispatchAsync(applicationMessage);

            // Assert
            await publisherMediatorMock
            .Received(1)
            .Publish(Arg.Is<object>(x => x.GetType() == notificationToDispatch.GetType())); 
        }
    }
}
