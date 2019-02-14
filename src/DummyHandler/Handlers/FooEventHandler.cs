using System;
using System.Threading.Tasks;
using DummyMessages;
using NServiceBus;

namespace DummyHandler.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus.Logging;

    public class FooEventHandler : IHandleMessages<FooEvent>
    {
        static ILog log = LogManager.GetLogger<FooEventHandler>();

        // Just send the BarEvent here
        public async Task Handle(FooEvent message, IMessageHandlerContext context)
        {
            log.Info($"Received FooEvent with ID {message.Id}, sleeping 30ms to simulate normal usage");

            await Task.Delay(30);

            var numberOfEventsToPublish = 10;
            var events = Enumerable.Range(1, numberOfEventsToPublish).Select(_ => new BarEvent {Id = Guid.NewGuid().ToString()});

            var tasks = new List<Task>(numberOfEventsToPublish);
            tasks.AddRange(events.Select(@event => context.Publish(@event)));

            await Task.WhenAll(tasks).ConfigureAwait(false);

            log.Info($"Published {numberOfEventsToPublish} BarEvent events.");
        }
    }
}