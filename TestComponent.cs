using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame
{
    public sealed record class TestComponent(int Value, Entity Entity) : ComponentBase(Entity)
    {
    }

    [RequiresComponent(typeof(TestComponent))]
    public sealed record class TestComponent2(Entity Entity) : ComponentBase(Entity)
    {
    }
}
