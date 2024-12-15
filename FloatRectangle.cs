using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame
{
    public record struct FloatRectangle
    {
        public Vector2 TopLeft;
        public Vector2 BottomRight;

        public Vector2 BottomLeft => new(TopLeft.X, BottomRight.Y);
        public Vector2 TopRight => new(BottomRight.X, TopLeft.Y);

        public float Left => TopLeft.X;
        public float Right => BottomRight.X;
        public float Top => TopRight.Y;
        public float Bottom => BottomRight.Y;

        public float Width => Right - Left;
        public float Height => Bottom - Top;

        public FloatRectangle(float centreX, float centreY, float width, float height)
        {
            TopLeft = new(centreX - width / 2, centreY - height / 2);
            BottomRight = new(centreX + width / 2, centreY + height / 2);
        }

        public FloatRectangle(Vector2 topLeft, Vector2 bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return Left <= point.X && point.X <= Right
                && Top <= point.Y && point.Y <= Bottom;
        }

        public bool OverlapsRectangle(FloatRectangle other)
        {
            return Left <= other.Right
                && Right >= other.Left
                && Top <= other.Bottom
                && Bottom >= other.Top;
        }
    }
}
