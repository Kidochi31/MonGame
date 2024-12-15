using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [RequiresComponent(typeof(Transform))]
    public sealed record class Camera(Entity Entity, float Width, float Height) : ComponentBase(Entity)
    {
        public float Width { get; set; } = Width;
        public float Height { get; set; } = Height;
        public RenderTarget2D RenderTarget { get; private set; }
        public int VirtualWidth { get; } = 1000;
        public int VirtualHeight { get; } = 1000;

        protected override void OnCreate(GameManager game)
        {
            RenderTarget = new RenderTarget2D(game.GraphicsDevice, VirtualWidth, VirtualHeight);
        }

        protected override void OnDestroy(GameManager game)
        {
            RenderTarget.Dispose();
        }
    }
}
