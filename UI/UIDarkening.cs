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
    public class UIDarkening : UpdateProcessor
    {
        public override void Update(GameTime gameTime, Ecs ecs, GameManager game)
        {
            foreach(DarkenOnHover darken in ecs.GetComponents<DarkenOnHover>())
            {
                // get the color and mouse over sensitivity
                UIColor uiColor = darken.Entity.GetComponent<UIColor>();
                ColorF color = uiColor.Color ?? ColorF.White;
                MouseOverSensitivity mouseOver = darken.Entity.GetComponent<MouseOverSensitivity>();
                // lerp if mouse over and it doesn't reach hover color
                if (mouseOver.HasMouseOver && color != darken.HoverColor)
                {
                    uiColor.Color = LerpBetweenColors(darken.DefaultColor, darken.HoverColor, color, darken.DarkenTime, gameTime.ElapsedGameTime.TotalSeconds);
                }
                else if (!mouseOver.HasMouseOver && color != darken.DefaultColor)
                {
                    uiColor.Color = LerpBetweenColors(darken.HoverColor, darken.DefaultColor, color, darken.UndarkenTime, gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }

        ColorF LerpBetweenColors(ColorF startColor, ColorF endColor, ColorF currentColor, double totalTime, double deltaTime)
        {
            return new ColorF(
                LerpBetweenFloats(startColor.R, endColor.R, currentColor.R, totalTime, deltaTime),
                LerpBetweenFloats(startColor.G, endColor.G, currentColor.G, totalTime, deltaTime),
                LerpBetweenFloats(startColor.B, endColor.B, currentColor.B, totalTime, deltaTime),
                LerpBetweenFloats(startColor.A, endColor.A, currentColor.A, totalTime, deltaTime)
                );
        }

        float LerpBetweenFloats(float start, float end, float current, double totalTime, double deltaTime)
        {
            if(start == end)
                return start;

            // find percentage of progress
            float difference = end - start;
            float progress = (current - start) / difference;
            float timeProgress = (float)(deltaTime / totalTime);
            // add on percentage of time to make the progress closer to 1
            float newProgress = progress < 1
                              ? progress + timeProgress
                              : progress - timeProgress;
            if (Math.Abs(newProgress - 1) < 0.0005)
                return end;

            float newCurrent = newProgress * difference + start;
            return newCurrent;
        }
    }
}
