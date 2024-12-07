using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class ComponentBase
    {
        public Entity Entity { get; }

        public ComponentBase(Entity entity)
        {
            Entity = entity;
            // Register component and add it to ecs
            Entity.Ecs.RegisterComponent(this);
            Entity.Ecs.GetEntity(Entity).AddComponent(this);
            OnCreate();
        }

        protected virtual void OnCreate() { }
        protected virtual void OnDestroy() { }

        // Removes this component from its entity and unregisters it from the ecs
        public void Destroy()
        {
            OnDestroy();

            Entity.Ecs.GetEntity(Entity).RemoveComponent(this);
            Entity.Ecs.UnregisterComponent(this);
        }
    }
}
