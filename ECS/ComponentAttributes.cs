using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    // Shows that this component requires the component 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RequiresComponentAttribute(Type componentType) : Attribute { public Type ComponentType = componentType; }
}
