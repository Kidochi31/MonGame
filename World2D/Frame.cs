using Microsoft.Xna.Framework.Graphics;
using MonGame.Drawing;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [RequiresComponent(typeof(Transform))]
    public sealed record class Frame(Entity Entity, float Width, float Height) : ComponentBase(Entity)
    {
        public float Width { get; set; } = Width;
        public float Height { get; set; } = Height;
    }
}
