using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    public enum ButtonState
    {
        HeldUp,
        Pressed,
        HeldDown,
        Released
    }

    public sealed record class Keybind(ButtonState KeyState, Keys Key, Type? Type, Entity Entity) : ComponentBase(Entity);
    public sealed record class MouseMovedBind(Type? Type, Entity Entity) : ComponentBase(Entity);
}
