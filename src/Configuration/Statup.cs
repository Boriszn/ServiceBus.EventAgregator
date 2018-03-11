using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusTopics;

namespace ServiceBus.EventAgregator.Configuration
{
    /// <summary>
    /// Bootstrap app configuration
    /// </summary>
    public static class Statup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Add IoC configuration
            serviceCollection.AddScoped<IServiceBusManager, ServiceBusManager>();
            serviceCollection.AddScoped<IEventAgregator, EventAgregator>();

            // Add file configuration options
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Add configuration options (for json config file)
            serviceCollection.AddOptions();
            serviceCollection.Configure<ServiceBusSettings>(configuration.GetSection("connectionStrings:serviceBus"));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
