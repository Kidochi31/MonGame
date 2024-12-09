using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    // This is what to use to reference components inside other components
    // This is for 1) safety and 2) to ensure components get garbage collected
    public readonly struct Component<T>(T component) where T : ComponentBase
    {
        public Entity Entity { get; } = component.Entity;
        public Type Type => typeof(T);
        public bool Exists() => Entity.Exists && Entity.HasComponent<T>();
        public T GetComponent() => Entity.GetComponent<T>();

        public static implicit operator Component<T>(T component)
        {
            return new Component<T>(component);
        }
    }
}
