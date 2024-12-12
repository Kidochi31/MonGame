using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.UI
{
    public static class UIRealPosition
    {
        public static Rectangle GetInitialRealFrame(GameManager game)
            => new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

        public static Rectangle VirtualFrameToRealFrame(Rectangle realFrame, Point virtualSize, Rectangle virtualFrame)
            => new Rectangle(VirtualPointToRealPoint(realFrame, virtualSize, new Point(virtualFrame.X, virtualFrame.Y)),
                             VirtualPointToRealPoint(realFrame, virtualSize, new Point(virtualFrame.X + virtualFrame.Width, virtualFrame.Y + virtualFrame.Height)));

        public static Point VirtualPointToRealPoint(Rectangle realFrame, Point virtualSize, Point virtualPoint)
            => new Point(virtualPoint.X / virtualSize.X * realFrame.Width + realFrame.X,
                         virtualPoint.Y / virtualSize.Y * realFrame.Height + realFrame.Y);

        public static Point RealPointToVirtualPoint(Rectangle realFrame, Point virtualSize, Point realPoint)
            => new Point((realPoint.X - realFrame.X) / realFrame.Width * virtualSize.X,
                         (realPoint.Y - realFrame.Y) / realFrame.Height * virtualSize.Y);
    }
}
