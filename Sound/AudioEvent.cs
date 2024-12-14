using MonGame.Assets;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Sound
{
    public abstract record class AudioEvent(Ecs? Ecs) : Event(Ecs);
    public record class PlaySoundEffectEvent(SoundEffectAsset soundEffect, Ecs? Ecs) : AudioEvent(Ecs);
}
