using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public readonly record struct Entity
    {
        public Ecs Ecs { get; }

        public Guid Guid { get; }
        public string Name { get; }
        public bool Exists => Ecs.EntityExists(this);

        internal Entity(UnderlyingEntity entity)
        {
            Ecs = entity.Ecs;
            Guid = entity.Guid;
            Name = entity.Name;
        }

        [Obsolete("You may not use the parameterless constructor.", error: true)]
        public Entity() => throw new InvalidOperationException("You may not use the parameterless constructor.");

        

        public bool HasComponent<T>()
            => GetEntity().Components?.Any(component => component is T) ?? throw new EntityNotFoundException();

        public T GetComponent<T>() where T : ComponentBase
             => (T)(GetEntity().Components?.FirstOrDefault(component => component is T) ?? throw new EntityNotFoundException());

        public List<T> GetComponents<T>()
            => (GetEntity().Components?.Where(component => component is T) ?? throw new EntityNotFoundException()).Cast<T>().ToList();

        private UnderlyingEntity GetEntity() => Ecs.GetEntity(this);
    }
}
