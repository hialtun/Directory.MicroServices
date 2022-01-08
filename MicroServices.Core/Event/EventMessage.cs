using System;

namespace MicroServices.Core.Event
{
    public abstract class EventMessage : IEvent
    {
        protected EventMessage()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; init; }
    }
}