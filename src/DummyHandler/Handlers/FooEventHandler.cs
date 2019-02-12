using System;
using System.IO;
using System.Threading;
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
            using (var ms = new MemoryStream())
            {
                Console.WriteLine($"Received Message with ID {message.Id}, sleeping 30ms seconds to simulate normal usage");
                Thread.Sleep(30);

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
                    Console.WriteLine($"Sent BarEvent with ID {barEvent.Id}");
                }
            }
        }
    }
}