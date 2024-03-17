using Messaging.PubSub.Publishers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TemperatureMeter.Application.Commands;
using TemperatureMeter.Domain.Converters;
using TemperatureMeter.Domain.Events;
using Xunit;

namespace TemperatureMeter.Application.Tests.Commands
{
    public class CreateTemperatureReadingCommandHandlerTests
    {
        [Fact] 
        public async Task Handle_GivenValidTemperatureReadingComand_PublishesTemperatureMeteringCreatedEvent() //MethodName_StateUnderTest_ExpectedBehavior
        {
            // Arrange 
            var cancellation = new CancellationToken();
            var temperatureInKelvin = 10d;
            var command = new CreateTemperatureReadingCommand() { DeviceId = new Guid("9910179C-2598-42F8-80EF-4BDA5087F130"), TemperatureInKelvin = temperatureInKelvin };

            var loggerMock = Substitute.For<ILogger<CreateTemperatureReadingCommandHandler>>();
            var messagePublisherFactoryMock = Substitute.For<IMessagePublisherFactory>();
            var kelvinToCelsiusConverterMock = Substitute.For<IKelvinToCelsiusConverter>();
            var publisherMock = Substitute.For<IMessagePublisher>();

            messagePublisherFactoryMock
            .GetOrCreateMessagePublisher(Arg.Any<string>(), Arg.Any<int>())
            .Returns(publisherMock);

            publisherMock.PublishAsync(Arg.Any<TemperatureMeteringCreated>(), Arg.Any<string>())
                .Returns(Task.FromResult(true));

            // Act
            var sut = new CreateTemperatureReadingCommandHandler(messagePublisherFactoryMock, kelvinToCelsiusConverterMock, loggerMock);
            var result = await sut.Handle(command, cancellation);

            // Assert
            kelvinToCelsiusConverterMock.Received(1).Convert(Arg.Is(temperatureInKelvin));
            await publisherMock
            .Received(1)
            .PublishAsync(Arg.Is<TemperatureMeteringCreated>(message => message.DeviceId == command.DeviceId), 
                Arg.Any<string>());
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_GivenInvalidTemperature_ReturnsFalseAndLogsIssue() 
        {
            // Arrange 
            var cancellation = new CancellationToken();
            var invalidTemperatureToConvert = -30d;
            var exception = new ArgumentOutOfRangeException($"Value '{invalidTemperatureToConvert}' can't be lesser than 0, because 0 is the absolute zero");
            var command = new CreateTemperatureReadingCommand() { DeviceId = new Guid("9910179C-2598-42F8-80EF-4BDA5087F130"), TemperatureInKelvin = invalidTemperatureToConvert };

            var loggerMock = Substitute.For<ILogger<CreateTemperatureReadingCommandHandler>>();
            var messagePublisherFactoryMock = Substitute.For<IMessagePublisherFactory>();
            var kelvinToCelsiusConverterMock = Substitute.For<IKelvinToCelsiusConverter>();
            kelvinToCelsiusConverterMock
                .Convert(Arg.Is<double>(invalidTemperatureToConvert))
                .Throws(exception);

            // Act
            var sut = new CreateTemperatureReadingCommandHandler(messagePublisherFactoryMock, kelvinToCelsiusConverterMock, loggerMock);
            var result = await sut.Handle(command, cancellation);

            // Assert
            Assert.False(result);
            kelvinToCelsiusConverterMock.Received(1).Convert(Arg.Is(invalidTemperatureToConvert));
            loggerMock.Received(1).LogError(exception, "Not able to process the CreateTemperatureReadingCommand");
            messagePublisherFactoryMock.DidNotReceive().GetOrCreateMessagePublisher(Arg.Any<string>(), Arg.Any<int>());
        }

        [Fact]
        public async Task Handle_GivenInvalidPublisherOptions_ReturnsFalseAndLogsIssue() 
        {
            // Arrange 
            var cancellation = new CancellationToken();
            var exception = new ArgumentException("The client options are not set.");
            var command = new CreateTemperatureReadingCommand() { DeviceId = new Guid("9910179C-2598-42F8-80EF-4BDA5087F130"), TemperatureInKelvin = 0 };

            var loggerMock = Substitute.For<ILogger<CreateTemperatureReadingCommandHandler>>();
            var messagePublisherFactoryMock = Substitute.For<IMessagePublisherFactory>();
            var kelvinToCelsiusConverterMock = Substitute.For<IKelvinToCelsiusConverter>();
            var publisherMock = Substitute.For<IMessagePublisher>();

            messagePublisherFactoryMock.GetOrCreateMessagePublisher(Arg.Any<string>(), Arg.Any<int>()).Throws(exception);

            // Act
            var sut = new CreateTemperatureReadingCommandHandler(messagePublisherFactoryMock, kelvinToCelsiusConverterMock, loggerMock);
            var result = await sut.Handle(command, cancellation);

            // Assert
            Assert.False(result);
            loggerMock.Received(1).LogError(exception, "Unable to publish TemperatureMeteringCreated");
        }
    }
}

