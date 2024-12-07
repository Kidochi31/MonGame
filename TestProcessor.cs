﻿using Microsoft.Xna.Framework;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame
{
    [UpdateProcessor]
    internal class TestProcessor() : Processor()
    {
        public override Type[] ReadComponents => [typeof(TestComponent)];

        public override void OnUpdate(GameTime gameTime, Ecs ecs)
        {
            Console.WriteLine($"frame: {gameTime.ElapsedGameTime}");
            Console.WriteLine($"{ecs.GetProcessor(typeof(TestProcessor))}");
        }
    }
}
