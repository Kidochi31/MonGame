using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class DrawProcessor : Processor
    {
        public virtual DrawLayer?[] Draw(GameTime gameTime, Ecs ecs, GameManager game) { return []; }
    }
}
