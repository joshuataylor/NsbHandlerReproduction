using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DummyMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace DummySender
{
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

            // Cinfigure ASB
            var transport = config.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString("");
            transport.TopicName("master");

            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);

            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            #region ClientLoop

            while (true)
            {
                var fooEvent = new FooEvent
                {
                    Id = Guid.NewGuid().ToString()
                };
                
                Console.WriteLine($"Sending event with ID {fooEvent.Id}");

                await endpointInstance.Publish(fooEvent)
                    .ConfigureAwait(false);
                
                Thread.Sleep(50);
            }

            #endregion
        }
    }
}