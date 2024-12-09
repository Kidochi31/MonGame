using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public readonly record struct DrawLayer(Texture2D Texture, Rectangle Source, Rectangle Destination, float Depth)
    {
        public void Draw(SpriteBatch spriteBatch)
        {
            (Texture2D texture, Rectangle source, Rectangle destination, float depth) = this;
            spriteBatch.Draw(texture, destination, source, Color.White, 0, Vector2.Zero, SpriteEffects.None, depth);
        }
    }
}
