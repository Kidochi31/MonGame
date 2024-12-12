using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    [FirstProcessor]
    public class Input : UpdateProcessor
    {
        KeyboardState OldKeyboardState;
        MouseState OldMouseState;
        readonly List<(ButtonState State, Keys Key, Type? Event)> InitialKeys = new(){
            (ButtonState.Released, Keys.A, typeof(PrintEvent))
        };
        
        readonly List<Type?> InitialMousePositions = new() {
            //typeof(PrintEvent)
        };

        EventConstructorCache<Func<Ecs, InputEvent>> CachedEventConstructors = new();

        internal override void Initialize(Ecs ecs, GameManager game)
        {
            OldKeyboardState = Keyboard.GetState();
            OldMouseState = Mouse.GetState();
            foreach((ButtonState State, Keys Key, Type? Event) in InitialKeys)
            {
                // create a keybind component on a new entity
                Entity keyEntity = ecs.CreateEntity();
                new Keybind(keyEntity, State, Key, Event);
            }
            foreach (Type? Event in InitialMousePositions)
            {
                // create a mouse position component on a new entity
                Entity keyEntity = ecs.CreateEntity();
                new MouseMovedBind(keyEntity, Event);
            }
        }

        public override void Update(GameTime gameTime, Ecs ecs, GameManager game)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();
            // go through all Keybind components
            foreach (Keybind keybind in ecs.GetComponents<Keybind>())
            {
                Type? type = keybind.Event;
                if (type is null || !ShouldInvokeKeyEvent(keybind, newKeyboardState))
                    continue;
                InvokeInputEvent(type, ecs);
            }

            bool mouseMoved = OldMouseState.Position != newMouseState.Position;
            // go through all mouse position bind components
            foreach (MouseMovedBind mouseMovedBind in ecs.GetComponents<MouseMovedBind>())
            {
                Type? type = mouseMovedBind.Event;
                if (type is null || !mouseMoved)
                    continue;

                InvokeInputEvent(type, ecs);
            }

            OldMouseState = newMouseState;
            OldKeyboardState = newKeyboardState;
        }

        private void InvokeInputEvent(Type type, Ecs ecs)
        {
            CachedEventConstructors[type](ecs);
        }

        private bool ShouldInvokeKeyEvent(Keybind keybind, KeyboardState newState)
        {
            switch (keybind.KeyState)
            {
                case ButtonState.HeldUp:
                    return newState.IsKeyUp(keybind.Key);
                case ButtonState.HeldDown:
                    return newState.IsKeyDown(keybind.Key);
                case ButtonState.Pressed:
                    return OldKeyboardState.IsKeyUp(keybind.Key) &&  newState.IsKeyDown(keybind.Key);
                case ButtonState.Released:
                    return OldKeyboardState.IsKeyDown(keybind.Key) && newState.IsKeyUp(keybind.Key);
            }
            return false;
        }
    }
}
