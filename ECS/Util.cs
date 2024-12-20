﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public static class Util
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var item in enumerable)
            {
                action(item);
            }
        }

        public static T InvokeEvent<T>(this T thisEvent, Ecs ecs) where T : Event
        {
            return Event.InvokeEvent(thisEvent, ecs);
        }
    }
}
