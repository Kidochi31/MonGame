using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BeforeProcessorAttribute(Type processor) : Attribute { public Type Processor { get; } = processor; }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AfterProcessorAttribute(Type processor) : Attribute { public Type Processor { get; } = processor; }

    [AttributeUsage(AttributeTargets.Class)]
    public class FirstProcessorAttribute() : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class LastProcessorAttribute() : Attribute { }
}
