using System;

namespace MicroServices.Core.Event
{
    public interface IEvent
    {
        Guid Id { get; init; }
    }
}