using System;
using System.Reflection;
using System.Threading.Tasks;
using DummyMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace DummySender
{
    using System.Collections.Generic;

    class Program
    {
        static async Task Main(string[] args)
        {
            LogManager.Use<DefaultFactory>()
                .Level(LogLevel.Info);
            var config = new EndpointConfiguration(Assembly.GetEntryAssembly().GetName().Name);
            config.EnableInstallers();
            config.UseSerialization<NewtonsoftSerializer>();
            config.SendFailedMessagesTo("error");
            config.AuditProcessedMessagesTo("audit");
            config.SendOnly();

            // Configure ASB
            var transport = config.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));

            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);

            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            var count = 2000;
            var numberOfMessages = count;
            var tasks = new List<Task>();

            while (count-- > 0)
            {
                var fooEvent = new FooEvent
                {
                    Id = Guid.NewGuid().ToString()
                };

                tasks.Add(endpointInstance.Publish(fooEvent));
            }

            Console.WriteLine($"Sending {numberOfMessages} messages...");
            await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Done.");
        }
    }
}