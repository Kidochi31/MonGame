﻿using Microsoft.Xna.Framework;
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
    [UpdateProcessor]
    [FirstProcessor]
    public class Input : Processor
    {
        KeyboardState OldKeyboardState;
        MouseState OldMouseState;
        readonly List<(ButtonState State, Keys Key, Type? Event)> InitialKeys = new(){
            (ButtonState.Released, Keys.A, typeof(PrintEvent))
        };
        
        readonly List<Type?> InitialMousePositions = new() {
            //typeof(PrintEvent)
        };

        Dictionary<Type, Func<Ecs, InputEvent>> CachedEventConstructors = [];

        public override void Initialize(Ecs ecs, GameManager game)
        {
            OldKeyboardState = Keyboard.GetState();
            OldMouseState = Mouse.GetState();
            foreach((ButtonState State, Keys Key, Type? Event) in InitialKeys)
            {
                // create a keybind component on a new entity
                Entity keyEntity = ecs.CreateEntity();
                new Keybind(State, Key, Event, keyEntity);
            }
            foreach (Type? Event in InitialMousePositions)
            {
                // create a mouse position component on a new entity
                Entity keyEntity = ecs.CreateEntity();
                new MouseMovedBind(Event, keyEntity);
            }
        }

        public override void OnUpdate(GameTime gameTime, Ecs ecs, GameManager game)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();
            // go through all Keybind components
            foreach (Keybind keybind in ecs.GetComponents<Keybind>())
            {
                Type? type = keybind.Type;
                if (type is null || !ShouldInvokeKeyEvent(keybind, newKeyboardState))
                    continue;
                InvokeInputEvent(type, ecs);
            }

            bool mouseMoved = OldMouseState.Position != newMouseState.Position;
            // go through all mouse position bind components
            foreach (MouseMovedBind mouseMovedBind in ecs.GetComponents<MouseMovedBind>())
            {
                Type? type = mouseMovedBind.Type;
                if (type is null || !mouseMoved)
                    continue;

                InvokeInputEvent(type, ecs);
            }
            OldMouseState = newMouseState;
            OldKeyboardState = newKeyboardState;
        }

        private void InvokeInputEvent(Type type, Ecs ecs)
        {
            if (CachedEventConstructors.ContainsKey(type))
            {
                CachedEventConstructors[type](ecs);
            }
            else
            {
                var ctor = type.GetConstructor([typeof(Ecs)]);
                var param = Expression.Parameter(typeof(Ecs), "ecs");
                // from Ecs to type
                Func<Ecs, InputEvent> constructor = (Func<Ecs, InputEvent>)Expression.Lambda(typeof(Func<,>).MakeGenericType([typeof(Ecs), type]), Expression.New(ctor, param), param).Compile();
                CachedEventConstructors.Add(type, constructor);
                constructor(ecs);
            }
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
