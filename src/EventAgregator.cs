using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBus.EventAgregator
{
    /// <inheritdoc />
    public class EventAgregator : IEventAgregator
    {
        private const string Subscription = "SubSetTest";

        private readonly IServiceBusManager serviceBusManager;
        private CancellationToken cancellationToken;

        /// <summary>
        /// Get and creates Message Subscribers configuration
        /// Notice: configuration based on labels. See example below
        /// </summary>
        /// <example>
        /// { "Label", MessageHandler },
        /// </example>
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
        /// First message handler example.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool DoFirstHandler(Message message)
        {
            // Get message body example
            var data = GetMessageBody(message);

            return true;
        }

        /// <summary>
        /// Second message handler example.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool DoSecondHandler(Message message)
        {
            // Get message body example
            var data = GetMessageBody(message);

            return true;
        }

        private static object GetMessageBody(Message message)
        {
            string text = Encoding.UTF8.GetString(message.Body);

            return JsonConvert.DeserializeObject(text);
        }
    }
}