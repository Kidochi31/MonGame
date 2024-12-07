using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    internal struct ComponentReference(ComponentBase component)
    {
        public Entity Entity { get; } = component.Entity;
        public Type 
    }
}
