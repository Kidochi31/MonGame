using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame
{
    public record struct ColorF(float R, float G, float B, float A)
    {
        public float R = R;
        public float G = G;
        public float B = B;
        public float A = A;

        public readonly static ColorF White = new(1, 1, 1, 1);
        public ColorF(Color color) : this(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f) { }

        public static explicit operator Color(ColorF color) { return new Color(color.R, color.G, color.B, color.A); }
        public static explicit operator ColorF(Color color) { return new ColorF(color); }
    }
}
