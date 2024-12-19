using Microsoft.Xna.Framework;
using MonGame.ECS;
using MonGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.UI
{
    [RequiresComponent(typeof(UIColor))]
    [RequiresComponent(typeof(MouseOverSensitivity))]
    public sealed record class DarkenOnHover(Entity Entity, ColorF DefaultColor, ColorF HoverColor, float DarkenTime, float UndarkenTime) : ComponentBase(Entity)
    {
        public ColorF DefaultColor = DefaultColor;
        public ColorF HoverColor = HoverColor;
        public float DarkenTime = DarkenTime;
        public float UndarkenTime = UndarkenTime;
    }
}
