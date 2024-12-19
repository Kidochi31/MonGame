using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.Drawing;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.UI
{
    public sealed record class UIColor(Entity Entity, ColorF? Color = null) : ComponentBase(Entity)
    {
        public ColorF? Color = Color;
    }
}
