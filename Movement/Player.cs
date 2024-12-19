using MonGame.ECS;
using MonGame.World2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Movement
{
    [RequiresComponent(typeof(Transform))]
    public sealed record class Player(Entity Entity) : ComponentBase(Entity)
    {
    }
}
