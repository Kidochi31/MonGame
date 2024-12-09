using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class Processor
    {
        public virtual bool IsActive { get; protected set; } = true;
        public virtual bool StopOnError => false;
        public virtual Type[] ReadComponents { get; } = [];
        public virtual Type[] ReadWriteComponents { get; } = [];
        public virtual Dictionary<Type, Func<Event, EventAction>> Events { get; } = [];
        public Processor() { }

        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
                OnDeactivate();
            }
        }
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                OnActivate();
            }
        }

        internal virtual void Initialize(Ecs ecs, GameManager game) { }
        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate() { }

        internal static bool AComesBeforeB(Type a, Type b)
        {
            // if a is before b or b is after a
            // or a is "first" (and b is not) or b is "last" (and a is not)
            return a.GetCustomAttributes<BeforeProcessorAttribute>().Any(attr => attr.Processor == b)
                || b.GetCustomAttributes<AfterProcessorAttribute>().Any(attr => attr.Processor == a)
                || a.GetCustomAttribute<FirstProcessorAttribute>() is not null && b.GetCustomAttribute<FirstProcessorAttribute>() is null
                || b.GetCustomAttribute<LastProcessorAttribute>() is not null && a.GetCustomAttribute<LastProcessorAttribute>() is not null;
        }

        internal static void InsertProcessorsIntoList<T>(IEnumerable<(Type Type, T Processor)> values, List<(Type Type, T Processor)> list) where T: Processor
        {
            foreach ((Type type, T processor) in values)
            {
                // find where to insert in list
                int i;
                for(i = 0; i < list.Count; i++)
                {
                    if (AComesBeforeB(type, list[i].Type))
                        break;
                }
                list.Insert(i, (type, processor));
            }
        }
    }
}
