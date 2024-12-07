using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    internal class UnderlyingEntity(Ecs ecs)
    {
        public Ecs Ecs = ecs;
        public Guid Guid = Guid.NewGuid();
        public List<ComponentBase>? Components = [];

        public void AddComponent(ComponentBase component)
        {
            if (Components is null)
                throw new EntityNotFoundException();
            if (Components.Any(c => c.GetType() == component.GetType()))
                throw new EntityHasComponentException(component);

            Components.Add(component);
        }

        public void RemoveComponent(ComponentBase component)
        {
            if (Components is null)
                throw new EntityNotFoundException();
            if (Components.All(c => c.GetType() != component.GetType()))
                throw new EntityDoesNotHaveComponentException(component);

            Components.Remove(component);
        }

        public void Destroy()
        {
            if (Components is null)
                throw new EntityNotFoundException();

            // Destroy all components
            foreach (ComponentBase component in Components)
                component.Destroy();

            // Remove component references
            Components = null;
        }
    }
}
