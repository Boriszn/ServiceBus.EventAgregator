using Moq;
using System;
using System.Text;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Xunit;

namespace ServiceBus.EventAgregator.Tests
{
    public class EventAgregatorTests
    {
        private readonly EventAgregator eventAgregator;

        public EventAgregatorTests()
        {
             eventAgregator = this.CreateEventAgregator();
        }

        [Fact]
        public void StartAgregatingMessages_NotThrowsAnyExceptions()
        {
            // Arrange

            // Act
            Action comparison = async () => { await eventAgregator.StartAgregating(); };

            // Assert
            comparison.ShouldNotThrow();
        }

        [Fact]
        public void DoFirstHandler_WithValidMessage_ReturnsTrue()
        {
            // Arrange
            var message = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@"{'Name':'James'}"))
            };

            // Act
            bool result = eventAgregator.DoFirstHandler(message);

            // Assert
            result.Should().BeTrue();
        }

        private EventAgregator CreateEventAgregator()
        {
            var mockServiceBusManager = new Mock<IServiceBusManager>();

            return new EventAgregator(mockServiceBusManager.Object);
        }
    }
}