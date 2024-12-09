using Microsoft.Xna.Framework.Graphics;
using MonGame.Drawing;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.UI
{
    [RequiresComponent(typeof(UITransform))]
    public sealed record class Frame(Entity Entity, int ActualWidth, int ActualHeight, int VirtualWidth = 1000, int VirtualHeight = 1000) : ComponentBase(Entity)
    {
        public int ActualWidth { get; set; } = ActualWidth;
        public int ActualHeight { get; set; } = ActualHeight;
        public int VirtualWidth { get; } = VirtualWidth;
        public int VirtualHeight { get; } = VirtualHeight;
        public RenderTarget2D RenderTarget { get; private set; }

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
