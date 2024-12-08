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
            // Register component and add it to ecs
            Entity.Ecs.RegisterComponent(this);
            Entity.Ecs.GetEntity(Entity).AddComponent(this);
        }

        // Removes this component from its entity and unregisters it from the ecs
        public void Destroy()
        {
            Entity.Ecs.GetEntity(Entity).RemoveComponent(this);
            Entity.Ecs.UnregisterComponent(this);
        }
    }
}
