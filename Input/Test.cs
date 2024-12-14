using MonGame.ECS;
using MonGame.Sound;
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
            {typeof(TestInputEvent), e => { Console.WriteLine("A"); new PlaySoundEffectEvent(e.Ecs.GameManager.SecretSound, e.Ecs); return EventAction.Continue; } }
        };
    }
}
