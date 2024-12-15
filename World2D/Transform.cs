using Microsoft.Xna.Framework;
using MonGame.ECS;
using MonGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    public sealed record class Transform(Entity Entity, Vector2 Position, float Depth) : ComponentBase(Entity)
    {
        public Vector2 Position { get; set; } = Position;
        public float Depth { get; set; } = Depth;
    }
}
