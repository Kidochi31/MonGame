using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class UpdateProcessor : Processor
    {
        public virtual void Update(GameTime gameTime, Ecs ecs, GameManager game) { }

        public virtual Dictionary<Type, Func<Event, EventAction>> Events { get; } = [];
    }
}
