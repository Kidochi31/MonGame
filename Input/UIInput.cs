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
using System.Threading.Tasks.Dataflow;

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

            Rectangle realFrame = UIRealPosition.GetInitialRealFrame(game);
            // go through all Gui components, then recursively find the frames/textures with
            IEnumerable<(Frame Frame, UIDepth Depth)> selectedFrames =
                (from gui in ecs.GetComponents<Gui>()
                 let virtualSize = new Point(gui.VirtualWidth, gui.VirtualHeight)
                 let transform = gui.Entity.GetComponent<UITransform>()
                 select GetFramesWithMouseOver(transform, new(transform.Depth), realFrame, virtualSize, newMouseState.Position)).SelectMany(x => x);

            selectedFrames = from frame in selectedFrames orderby frame.Depth descending select frame;
            foreach ((Frame frame, _) in selectedFrames)
            {
                //Console.WriteLine(frame.Entity.Name);
                if (frame.Entity.HasComponent<UIMouseBind>())
                {
                    UIMouseBind bind = frame.Entity.GetComponent<UIMouseBind>();
                    foreach ((UIMouseEvent mouseEvent, InputEvent? Event) in bind.Events)
                    {
                        if (IsCurrentMouseEvent(mouseEvent, newMouseState) && Event is not null)
                        {
                            Event.InvokeEvent(ecs);
                        }
                    }
                }

                if (frame.Entity.HasComponent<UIMouseBlock>())
                    break;

                // stop if it reaches a mouse block
            }


            OldMouseState = newMouseState;
        }

        private IEnumerable<(Frame Frame, UIDepth Depth)> GetFramesWithMouseOver(UITransform transform, UIDepth depth, Rectangle realFrame, Point virtualSize, Point realMousePosition)
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
                yield return (childFrame, new(depth, childTransform.Depth));
                foreach (var grandChildFrame in GetFramesWithMouseOver(childTransform, new(depth, childTransform.Depth), childRealFrame, new Point(childFrame.VirtualWidth, childFrame.VirtualHeight), realMousePosition))
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
