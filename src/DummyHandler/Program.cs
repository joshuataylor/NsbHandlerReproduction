using System;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace DummyHandler
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
            config.LimitMessageProcessingConcurrencyTo(10);

            // Cinfigure ASB
            var transport = config.UseTransport<AzureServiceBusTransport>();
            transport.ConnectionString("");
            transport.TopicName("master");

            var endpointInstance = await Endpoint.Start(config)
                .ConfigureAwait(false);
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}