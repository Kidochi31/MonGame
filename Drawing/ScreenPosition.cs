using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Drawing
{
    public record class ScreenPosition(Entity Entity) : ComponentBase(Entity)
    {
    }
}
