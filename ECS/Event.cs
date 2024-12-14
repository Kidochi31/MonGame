using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract record class Event
    {
        public Ecs? Ecs { get; init; }

        // null if the event is a template (i.e. to be used by InvokeEvent)
        public Event(Ecs? ecs)
        {
            Ecs = ecs;
            Ecs?.RegisterEvent(this);
        }

        public void Destroy()
        {
            Ecs?.DestroyEvent(this);
        }

        public static T InvokeEvent<T>(T Event, Ecs ecs) where T: Event
        {
            if (Event.Ecs is not null)
                throw new EventAlreadyCreatedException(Event);
            T newEvent = Event with { Ecs = ecs };
            ecs.RegisterEvent(newEvent);
            return newEvent;
        }
    }

    public enum EventAction
    {
        Consume,
        Continue
    }
}
