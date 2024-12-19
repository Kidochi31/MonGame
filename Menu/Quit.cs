using MonGame.ECS;
using MonGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Menu
{
    [LastProcessor]
    public class Quit : UpdateProcessor
    {
        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = new() {
            { typeof(QuitEvent), e => {e.Ecs.GameManager.Exit(); return EventAction.Consume; } }
        };
    }
}
