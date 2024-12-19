using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.UI;
using MonGame.World2D;
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

    public sealed record class Keybind(Entity Entity, ButtonState KeyState, Keys Key, InputEvent? Event) : ComponentBase(Entity)
    {
        public ButtonState KeyState { get; set; } = KeyState;
        public Keys Key { get; set; } = Key;
        public InputEvent Event { get; set; } = Event;
    }
    public sealed record class MouseMovedBind(Entity Entity, InputEvent? Event) : ComponentBase(Entity)
    {
        public InputEvent? Event { get; set; } = Event;
    }

    public enum UIMouseEvent
    {
        MouseOver,
        LeftMouseDown,
        RightMouseDown,
        LeftMouseClick,
        RightMouseClick,
    }

    public sealed record class MouseBind(Entity Entity, List<(UIMouseEvent MouseEvent, InputEvent? Event)> Events) : ComponentBase(Entity)
    {
        public List<(UIMouseEvent MouseEvent, InputEvent? Event)> Events { get; } = Events;
    }

    public sealed record class MouseOverSensitivity(Entity Entity) : ComponentBase(Entity)
    {
        public bool HasMouseOver = false;
    }

    public sealed record class MouseBlock(Entity Entity) : ComponentBase(Entity);
}
