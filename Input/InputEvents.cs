using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    public abstract record class InputEvent(Ecs? Ecs) : Event(Ecs);

    public record class PrintEvent(Ecs? Ecs) : InputEvent(Ecs);

}
