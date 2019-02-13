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
                Console.WriteLine($"Sending BarEvent with ID {barEvent.Id}");
                await context.Publish(barEvent).ConfigureAwait(false);
                Console.WriteLine($"Published BarEvent to send with ID {barEvent.Id}");
            }
        }
    }
}