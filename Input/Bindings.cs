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

    public sealed record class Keybind(Entity Entity, ButtonState KeyState, Keys Key, Type? Event) : ComponentBase(Entity)
    {
        public ButtonState KeyState { get; set; } = KeyState;
        public Keys Key { get; set; } = Key;
        public Type? Event { get; set; } = Event;
    }
    public sealed record class MouseMovedBind(Entity Entity, Type? Event) : ComponentBase(Entity)
    {
        public Type? Event { get; set; } = Event;
    }
}
