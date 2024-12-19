using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    public abstract record class InputEvent(Ecs? Ecs) : Event(Ecs);

    public record class TestInputEvent(Ecs? Ecs) : InputEvent(Ecs);

    public record class MoveLeftEvent(Ecs? Ecs) : InputEvent(Ecs);
    public record class MoveRightEvent(Ecs? Ecs) : InputEvent(Ecs);
    public record class MoveUpEvent(Ecs? Ecs) : InputEvent(Ecs);
    public record class MoveDownEvent(Ecs? Ecs) : InputEvent(Ecs);

    public record class PauseEvent(Ecs? Ecs) : InputEvent(Ecs);
    public record class UnpauseEvent(Ecs? Ecs) : InputEvent(Ecs);
    public record class QuitEvent(Ecs? Ecs) : InputEvent(Ecs);

}
