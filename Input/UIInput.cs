using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Input
{
    [FirstProcessor]
    public class UIInput : UpdateProcessor
    {
        MouseState OldMouseState;

        internal override void Initialize(Ecs ecs, GameManager game)
        {
            OldMouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime, Ecs ecs, GameManager game)
        {
            MouseState newMouseState = Mouse.GetState();

            // go through all Gui components, then recursively find the frames/textures with
            


            OldMouseState = newMouseState;
        }

        private bool ShouldInvokeUIMouseBindEvent(UIMouseBind uiBind, MouseState newState)
        {
            //if (!IsCurrentMouseEvent(uiBind, newState))
            //    return false;

            // Check the position
            return false;
        }

        private IEnumerable<Frame> GetFramesWithMouseOver(UITransform transform, Rectangle realFrame, Point virtualSize, Point realMousePosition)
        {
            // find all children, check if their real frames contain the realMousePosition
            foreach (Component<UITransform> child in transform.Children)
            {
                UITransform childTransform = child.GetComponent();
                if (!childTransform.Entity.HasComponent<Frame>())
                    continue;
                Frame childFrame = childTransform.Entity.GetComponent<Frame>();
                Rectangle childVirtualFrame = new Rectangle(childTransform.Position, new Point(childFrame.ActualWidth, childFrame.ActualHeight));
                Rectangle childRealFrame = UIRealPosition.VirtualFrameToRealFrame(realFrame, virtualSize, childVirtualFrame);
                if (!childRealFrame.Contains(realMousePosition))
                    continue;

                // return the child and all valid grand children
                yield return childFrame;
                foreach (Frame grandChildFrame in GetFramesWithMouseOver(childTransform, childRealFrame, new Point(childFrame.VirtualWidth, childFrame.VirtualHeight), realMousePosition))
                    yield return grandChildFrame;

            }
            yield break;
        }

        private bool IsCurrentMouseEvent(UIMouseEvent mouseEvent, MouseState newState)
        {
            switch (mouseEvent)
            {
                case UIMouseEvent.MouseOver:
                    return true;
                case UIMouseEvent.LeftMouseDown:
                    return newState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                case UIMouseEvent.RightMouseDown:
                    return newState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                case UIMouseEvent.LeftMouseClick:
                    return newState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released
                        && OldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
                case UIMouseEvent.RightMouseClick:
                    return newState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released
                        && OldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            }
            return false;
        }
    }
}
