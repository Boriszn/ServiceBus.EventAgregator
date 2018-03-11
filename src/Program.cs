using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.EventAgregator.Configuration;

namespace ServiceBus.EventAgregator
{
    class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // Bootstraps application configuration and Run Main async entry point
            MainAsync(args, 
                Statup.ConfigureServices())
                .GetAwaiter()
                .GetResult();
        }

        private static async Task MainAsync(string[] args, IServiceProvider serviceProvider)
        {
            var serviceBusManager = serviceProvider.GetService<IServiceBusManager>();

            // 1. Start listening and Aggregating messages
            await AggregateMessages(serviceProvider);

            // 2. Sends Test messages to different handlers (with different labels)
            await SendMessages(serviceBusManager);

            // 3. Wait a bit and show message to console
            await Task.Delay(5);
            Console.WriteLine($" Messages sent.");

            Console.ReadKey();

            // Close the client after the ReceiveMessages method has exited.

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static async Task SendMessages(IServiceBusManager serviceBusManager)
        {
            string label1 = "First";
            string json1 = @"{'Name':'James'}";

            string label2 = "Second";
            string json2 = @"{'Name':'James'}";

            await serviceBusManager.SendMessage(label1, json1);

            await Task.Delay(3);

            await serviceBusManager.SendMessage(label2, json2);
        }

        private static async Task AggregateMessages(IServiceProvider serviceProvider)
        {
            var eventAgregator = serviceProvider.GetService<IEventAgregator>();

            await Task.Factory.StartNew(async () =>
            {
                await eventAgregator.StartAgregating();
            });
        }
    }
}