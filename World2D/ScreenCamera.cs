using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [RequiresComponent(typeof(Camera))]
    public sealed record class ScreenCamera(Entity Entity, float WorldToScreenScale) : ComponentBase(Entity)
    {
        public float WorldToScreenScale { get; set; } = WorldToScreenScale;
    }
}
