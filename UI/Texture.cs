using MonGame.Assets;
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
    public sealed record class Texture(Entity Entity, Texture2DAsset Asset, int Width, int Height) : ComponentBase(Entity)
    {
        public Texture2DAsset Asset { get; set; } = Asset;
        public int Width { get; set; } = Width;
        public int Height { get; set; } = Height;
    }
}
