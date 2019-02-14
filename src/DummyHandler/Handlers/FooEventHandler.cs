using System;
using System.Threading.Tasks;
using DummyMessages;
using NServiceBus;

namespace DummyHandler.Handlers
{
    using NServiceBus.Logging;

    public class FooEventHandler : IHandleMessages<FooEvent>
    {
        static ILog log = LogManager.GetLogger<FooEventHandler>();

        // Just send the BarEvent here
        public async Task Handle(FooEvent message, IMessageHandlerContext context)
        {
            log.Info($"Received Message with ID {message.Id}, sleeping 30ms to simulate normal usage");
            await Task.Delay(30);

            var i = 0;
            // Send 10 events from here
            while (i <= 10)
            {
                var barEvent = new BarEvent
                {
                    Id = Guid.NewGuid().ToString()
                };

                i++;
                await context.Publish(barEvent).ConfigureAwait(false);
            }
            log.Info("Published 10 BarEvent events.");
        }
    }
}