using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public readonly struct Entity
    {
        public Ecs Ecs { get; }
        public Guid Guid { get; }

        internal Entity(UnderlyingEntity entity)
        {
            Ecs = entity.Ecs;
            Guid = entity.Guid;
        }

        [Obsolete("You may not use the parameterless constructor.", error: true)]
        public Entity() => throw new InvalidOperationException("You may not use the parameterless constructor.");

        public bool HasComponent<T>() where T : ComponentBase
            => GetEntity().Components?.Any(component => component is T) ?? throw new EntityNotFoundException();

        public T GetComponent<T>() where T : ComponentBase
             => (T)(GetEntity().Components?.FirstOrDefault(component => component is T) ?? throw new EntityNotFoundException());

        private UnderlyingEntity GetEntity() => Ecs.GetEntity(this);
    }
}
