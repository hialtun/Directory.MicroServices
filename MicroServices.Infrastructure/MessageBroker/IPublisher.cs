using MicroServices.Core.Event;

namespace MicroServices.Infrastructure.MessageBroker
{
    public interface IPublisher<TEvent> where TEvent : IEvent
    {
        void Publish(TEvent @event);
    }
}