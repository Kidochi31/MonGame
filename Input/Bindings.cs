using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.UI;
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

    public enum UIMouseEvent
    {
        MouseOver,
        LeftMouseDown,
        RightMouseDown,
        LeftMouseClick,
        RightMouseClick,
    }

    [RequiresComponent(typeof(UITransform))]
    [RequiresComponent(typeof(Frame))]
    public sealed record class UIMouseBind(Entity Entity, List<(UIMouseEvent MouseEvent, Type? Event)> Events) : ComponentBase(Entity)
    {
        public List<(UIMouseEvent MouseEvent, Type? Event)> Events { get; } = Events;
    }


    [RequiresComponent(typeof(UITransform))]
    [RequiresComponent(typeof(Frame))]
    public sealed record class UIMouseBlock(Entity Entity) : ComponentBase(Entity);
}
