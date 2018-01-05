using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBus.EventAgregator
{
    /// <summary>
    /// The Service Bus manager contains logic for Subscribing on messages and Send messages
    /// TODO: NOTICE: Works with service bus topics. Logic to work with Queues will be added later.
    /// </summary>
    public interface IServiceBusManager
    {
        /// <summary>
        /// Registers the on receive messages.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="subscriptionToLabelHandler">The subscription to labe handler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterOnReceiveMessages(string subscription, Dictionary<string, Func<Message, bool>> subscriptionToLabelHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="messageContent">Content of the message.</param>
        /// <returns></returns>
        Task SendMessage(string label, string messageContent);
    }
}