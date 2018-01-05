using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceBusTopics;

namespace ServiceBus.EventAgregator
{
    /// <inheritdoc/>
    public class ServiceBusManager : IServiceBusManager
    {
        private readonly ServiceBusSettings serviceBusSettings;

        public ServiceBusManager(IOptions<ServiceBusSettings> config)
        {
            serviceBusSettings = config.Value;
        }

        // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
        private MessageHandlerOptions MessageHandlerOptions => new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            MaxConcurrentCalls = 1,
            
            // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
            AutoComplete = false,
        };

        /// <inheritdoc/>
        public async Task RegisterOnReceiveMessages(string subscription, Dictionary<string, Func<Message, bool>> subscriptionToLabelHandler, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            SubscriptionClient subscriptionClient = GetSubscriptionClient(subscription);

            RegisterCancellationToken(cancellationToken, subscriptionClient, taskCompletionSource);

            var messageHandlerOptions = MessageHandlerOptions;

            // Register the function that will process messages
            subscriptionClient.RegisterMessageHandler(async (message, token) =>
            {
                // Process the message
                Console.WriteLine($"Received message: SequenceNumber:{message.Label} | SequenceNumber:{message.SystemProperties.SequenceNumber} | Body:{Encoding.UTF8.GetString(message.Body)}");

                subscriptionToLabelHandler[message.Label](message);

                // Complete the message so that it is not received again.
                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            }, messageHandlerOptions);

            await taskCompletionSource.Task;
        }

        /// <inheritdoc/>
        public async Task SendMessage(string label, string messageContent)
        {
            try
            {
                var topicClient = new TopicClient(serviceBusSettings.ConnectionString, serviceBusSettings.TopicName);

                var messageData = GetMessageContent(label, messageContent);

                var message = new Message
                {
                    Body = messageData,
                    Label = label,
                };

                // Send the message to the queue
                await topicClient.SendAsync(message);

                await topicClient.CloseAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }
        }

        private static byte[] GetMessageContent(string label, string messageContent)
        {
            byte[] messageData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            // Write the body of the message to the console
            Console.WriteLine($"Sending message: {label} | {Encoding.UTF8.GetString(messageData)}");

            return messageData;
        }

        private SubscriptionClient GetSubscriptionClient(string subscription)
        {
            return new SubscriptionClient(serviceBusSettings.ConnectionString, serviceBusSettings.TopicName, subscription);
        }

        private static void RegisterCancellationToken(CancellationToken cancellationToken, SubscriptionClient subscription, TaskCompletionSource<bool> doneReceiving)
        {
            cancellationToken.Register(
                async () =>
                {
                    await subscription.CloseAsync();
                    doneReceiving.SetResult(true);
                });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
    }
}