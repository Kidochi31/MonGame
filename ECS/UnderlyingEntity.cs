using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    internal class UnderlyingEntity(Ecs ecs, Guid guid, string name)
    {
        public Ecs Ecs = ecs;
        public Guid Guid = guid;
        public string Name = name;
        public List<ComponentBase>? Components = [];

        private bool BeingDestroyed = false;
        
        void AssertComponentRequirements(ComponentBase component)
        {
            if (Components is null)
                throw new EntityNotFoundException();
            var requirements = component.GetType().GetCustomAttributes<RequiresComponentAttribute>();
            if (requirements != null)
            {
                foreach (var requirement in requirements)
                {
                    if (requirement is null)
                        continue;
                    if (!Components.Any(c => c.GetType().IsAssignableTo(requirement.ComponentType)))
                        throw new ComponentRequirementNotMetException(requirement.ComponentType);
                }
            }
        }

        public void AddComponent(ComponentBase component)
        {
            
            // check no other components of the same type
            if (Components.Any(c => c.GetType() == component.GetType()))
                throw new EntityHasComponentException(component);

            // check all required components are on
            AssertComponentRequirements(component);

            Components.Add(component);
        }

        public void RemoveComponent(ComponentBase component)
        {
            if (Components is null)
                throw new EntityNotFoundException();
            if (Components.All(c => !ReferenceEquals(c, component)))
                throw new EntityDoesNotHaveComponentException(component);

            Components.Remove(component);

            // also check that all other components don't have this as a requirement
            // if they do, add it back (and throw an exception)
            // skip this if the entity is being destroyed
            if (BeingDestroyed)
                return;
            try
            {
                foreach (ComponentBase c in Components)
                    AssertComponentRequirements(c);
            }
            catch(Exception e)
            {
                Components.Add(component);
                throw;
            }
        }

        public void Destroy()
        {
            if (Components is null)
                throw new EntityNotFoundException();
            BeingDestroyed = true;

            // Destroy all components
            foreach(ComponentBase component in Components.ToList())
                component.Destroy();

            // Remove component references
            Components = null;
        }
    }
}
