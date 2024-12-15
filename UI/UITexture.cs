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
    [RequiresComponent(typeof(UIFrame))]
    public sealed record class UITexture(Entity Entity, Texture2DAsset Asset) : ComponentBase(Entity)
    {
        public Texture2DAsset Asset { get; set; } = Asset;
    }
}
