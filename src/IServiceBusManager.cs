using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusTopics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceBusManager
    {
        /// <summary>
        /// Registers the on receive messages.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="subscriptionToLabeHandler">The subscription to labe handler.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterOnReceiveMessages(string subscription, Dictionary<string, Func<Message, bool>> subscriptionToLabeHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="numMessagesToSend">The number messages to send.</param>
        /// <param name="label">The label.</param>
        /// <param name="messageContent">Content of the message.</param>
        /// <returns></returns>
        Task SendMessage(string label, string messageContent);
    }
}