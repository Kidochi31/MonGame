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

    public enum EventAction
    {
        Consume,
        Continue
    }
}
