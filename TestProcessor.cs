using Microsoft.Xna.Framework;
using MonGame.Drawing;
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
        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = new() {
            { typeof(TestEvent), e =>
            {
                TestEvent t = (TestEvent)e;
                Console.WriteLine(t.Value);
                return EventAction.Continue;
            } } };

        public override void OnUpdate(GameTime gameTime, Ecs ecs, GameManager gameManager)
        {
            new TestEvent(1, ecs);
            new TestEvent(1, ecs);
            new TestEvent(2, ecs);
        }
    }

    public record class TestEvent(int Value, Ecs Ecs) : Event(Ecs) { }
}
