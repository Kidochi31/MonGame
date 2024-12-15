using MonGame.Assets;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [RequiresComponent(typeof(Transform))]
    [RequiresComponent(typeof(Frame))]
    public sealed record class Texture(Entity Entity, Texture2DAsset Asset) : ComponentBase(Entity)
    {
        public Texture2DAsset Asset { get; set; } = Asset;
    }
}
