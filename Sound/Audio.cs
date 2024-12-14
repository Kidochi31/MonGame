using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Sound
{
    public class Audio : UpdateProcessor
    {
        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = new()
        {
            {typeof(PlaySoundEffectEvent), e => { ((PlaySoundEffectEvent)e).soundEffect.SoundEffect.Play(); return EventAction.Continue; } }
        };
    }
}
