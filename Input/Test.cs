using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    public class Test : UpdateProcessor
    {
        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = new Dictionary<Type, Func<Event, EventAction>>
        {
            {typeof(PrintEvent), e => { Console.WriteLine("A"); return EventAction.Continue; } }
        };
    }
}
