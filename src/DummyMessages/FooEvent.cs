using NServiceBus;

namespace DummyMessages
{
    public class FooEvent : IEvent
    {
        public string Id { get; set; }
    }
}