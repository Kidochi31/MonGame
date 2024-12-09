using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract record class Event
    {
        Ecs Ecs { get; }
        public Event(Ecs ecs)
        {
            Ecs = ecs;
            Ecs.RegisterEvent(this);
        }

        public void Destroy()
        {
            Ecs.DestroyEvent(this);
        }
    }

    public abstract record class DrawEvent
    {
        Ecs Ecs { get; }
        public DrawEvent(Ecs ecs)
        {
            Ecs = ecs;
            Ecs.RegisterDrawEvent(this);
        }

        public void Destroy()
        {
            Ecs.DestroyDrawEvent(this);
        }
    }

    public enum EventAction
    {
        Consume,
        Continue
    }
}
