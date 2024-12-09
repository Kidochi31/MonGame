using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    // This is the class to inherit from when creating new component types
    // DO NOT reference ComponentBase (or any other component) as a field of a component
    public abstract record class ComponentBase
    {
        public Entity Entity { get; }

        public ComponentBase(Entity entity)
        {
            Entity = entity;
            // Register component and add it to ecs (and entity)
            Entity.Ecs.RegisterComponent(this, entity);
            OnCreate(Entity.Ecs.GameManager);
        }

        protected virtual void OnCreate(GameManager game) { }
        protected virtual void OnDestroy(GameManager game) { }

        // Removes this component from its entity and unregisters it from the ecs
        public void Destroy()
        {
            OnDestroy(Entity.Ecs.GameManager);
            Entity.Ecs.UnregisterComponent(this);
        }
    }
}
