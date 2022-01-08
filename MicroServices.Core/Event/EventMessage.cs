using System;

namespace MicroServices.Core.Event
{
    public abstract class EventMessage : IEvent
    {
        protected EventMessage()
        {
            Id = new Guid();
        }
        public Guid Id { get; init; }
    }
}