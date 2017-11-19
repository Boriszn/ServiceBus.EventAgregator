using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBusTopics;

namespace ServiceBus.EventAgregator
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ServiceBusTopics.IEventAgregator" />
    public class EventAgregator : IEventAgregator
    {
        private const string Subscription = "SubSetTest";

        private readonly IServiceBusManager serviceBusManager;
        private CancellationToken cancellationToken;

        /// <summary>
        /// Gets the subscription to label funcs.
        /// </summary>
        /// <value>
        /// The subscription to label funcs.
        /// </value>
        private Dictionary<string, Func<Message, bool>> SubscriptionToLabelFuncs => new Dictionary<string, Func<Message, bool>>
        {
            { "First", DoFirstHandler },
            { "Second", DoSecondHandler }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAgregator"/> class.
        /// </summary>
        /// <param name="serviceBusManager">The service bus manager.</param>
        public EventAgregator(IServiceBusManager serviceBusManager)
        {
            // Subscribe on events
            this.serviceBusManager = serviceBusManager;
        }

        /// <summary>
        /// Starts the agregating.
        /// </summary>
        /// <returns></returns>
        public async Task StartAgregating()
        {
            this.cancellationToken = new CancellationTokenSource().Token;
            await serviceBusManager.RegisterOnReceiveMessages(Subscription, SubscriptionToLabelFuncs, cancellationToken);
        }

        /// <summary>
        /// Firsts the handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool DoFirstHandler(Message message)
        {
            return true;
        }

        /// <summary>
        /// Does the second handler.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool DoSecondHandler(Message message)
        {
            return true;
        }
    }
}