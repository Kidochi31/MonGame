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
        public virtual Type[] ReadComponents { get; } = [];
        public virtual Type[] ReadWriteComponents { get; } = [];
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

        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
        public virtual void OnUpdate(GameTime gameTime, Ecs ecs) { }

        public static bool AComesBeforeB(Type a, Type b)
        {
            // if a is before b or b is after a
            return a.GetCustomAttributes<BeforeProcessorAttribute>().Any(attr => attr.Processor == b)
                || b.GetCustomAttributes<AfterProcessorAttribute>().Any(attr => attr.Processor == a);
        }

        public static void InsertProcessorsIntoList(IEnumerable<(Type Type, Processor Processor)> values, List<(Type Type, Processor Processor)> list)
        {
            foreach ((Type type, Processor processor) in values)
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
