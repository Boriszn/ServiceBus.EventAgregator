using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusTopics;

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
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Endpoint
            MainAsync(args, serviceProvider).GetAwaiter().GetResult();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add IoC configuration
            serviceCollection.AddScoped<IServiceBusManager, ServiceBusManager>();
            serviceCollection.AddScoped<IEventAgregator, EventAgregator>();

            // Add file configuration options
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<ServiceBusSettings>(configuration.GetSection("connectionStrings:serviceBus"));
        }

        private static async Task MainAsync(string[] args, IServiceProvider serviceProvider)
        {
            var serviceBusManager = serviceProvider.GetService<IServiceBusManager>();

            await AggregateMessages(serviceProvider);

            await SendMessages(serviceBusManager);

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

            await Task.Factory.StartNew(async () => { await eventAgregator.StartAgregating(); });
        }
    }
}