using Microsoft.Xna.Framework;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Drawing
{
    [DrawProcessor]
    [LastProcessor]
    public class FinalizeDrawing : Processor
    {
        public override bool StopOnError => true;
        public override void OnUpdate(GameTime gameTime, Ecs ecs, GameManager gameManager)
        {
            gameManager.SpriteBatch.End();
        }
    }
}
