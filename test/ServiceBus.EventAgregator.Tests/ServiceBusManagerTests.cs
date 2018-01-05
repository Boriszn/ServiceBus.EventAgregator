using Microsoft.Extensions.Options;
using ServiceBusTopics;
using System;
using FluentAssertions;
using Xunit;

namespace ServiceBus.EventAgregator.Tests
{
    public class ServiceBusManagerTests
    {
        private readonly ServiceBusManager serviceBusManager;

        public ServiceBusManagerTests()
        {
            serviceBusManager = this.CreateManager();
        }

        [Fact(Skip = "The valid 'ServiceBusOptions' should be filled first")]
        public void SendMessage_WithMessageLabelAndContent_NotThrowsAnyException()
        {
            // Arrange
            string messageLabel = "TestLabel";
            string messageContent = @"{'Name':'James'}";

            // Act
            Action comparison = async () => { await serviceBusManager.SendMessage(messageLabel, messageContent); };

            // Assert
            comparison.ShouldNotThrow<Exception>();
        }

        private ServiceBusManager CreateManager()
        {
            IOptions<ServiceBusSettings> serviceBusOptions = Options.Create(new ServiceBusSettings
            {
                ConnectionString = "Endpoint=sb://sbusserver.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<key>",
                TopicName = "test"
            });

            //var topic = new Mock<ITopicClient>();

            return new ServiceBusManager(serviceBusOptions);
        }
    }
}