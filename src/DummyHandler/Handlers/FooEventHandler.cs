using System;
using System.Threading.Tasks;
using DummyMessages;
using NServiceBus;

namespace DummyHandler.Handlers
{
    public class FooEventHandler : IHandleMessages<FooEvent>
    {
        // Just send the BarEvent here
        public async Task Handle(FooEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received Message with ID {message.Id}, sleeping 30ms to simulate normal usage");
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
            Console.WriteLine($"Published 10 BarEvent events.");
        }
    }
}