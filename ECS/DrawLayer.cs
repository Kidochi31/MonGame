using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public record struct DrawLayer(Texture2D Texture, Rectangle Source, Rectangle Destination, float Depth, ColorF? Color = null)
    {
        public Texture2D Texture = Texture;
        public Rectangle Source = Source;
        public Rectangle Destination = Destination;
        public float Depth = Depth;
        public ColorF? Color = Color;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Destination, Source, (Color)(Color ?? ColorF.White), 0, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
