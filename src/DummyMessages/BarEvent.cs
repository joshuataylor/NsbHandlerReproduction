using NServiceBus;

namespace DummyMessages
{
    public class BarEvent : IEvent
    {
        public string Id { get; set; }
    }
}