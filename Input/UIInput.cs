using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.UI;
using MonGame.World2D;
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

        public override void Initialize(Ecs ecs, GameManager game)
        {
            OldMouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime, Ecs ecs, GameManager game)
        {
            MouseState newMouseState = Mouse.GetState();
            if (!MouseOnScreen(newMouseState, game))
            {
                OldMouseState = newMouseState;
                return;
            }

            bool foundBlock = false;

            Rectangle realFrame = UIRealPosition.GetInitialRealFrame(game);

            // set all mouse sensitivities to false
            ecs.GetComponents<MouseOverSensitivity>().ForEach(x => x.HasMouseOver = false);

            // go through all Gui components, then recursively find the frames/textures with the mouse
            // only use active frames
            IEnumerable<(UIFrame Frame, UIDepth Depth)> selectedFrames =
                (from gui in ecs.GetComponents<Gui>()
                 let virtualSize = new Point(gui.VirtualWidth, gui.VirtualHeight)
                 let transform = gui.Entity.GetComponent<UITransform>()
                 where transform.Active
                 select GetFramesWithMouseOver(transform, new(transform.Depth), realFrame, virtualSize, newMouseState.Position)).SelectMany(x => x);

            selectedFrames = from frame in selectedFrames orderby frame.Depth descending select frame;
            foreach ((UIFrame frame, _) in selectedFrames)
            {
                InvokeMouseBindings(ecs, frame, ref foundBlock, newMouseState);

                if (foundBlock)
                    break;
            }

            if (foundBlock)
            {
                OldMouseState = newMouseState;
                return;
            }

            // if it hasn't reached a mouse block, go through all MouseBlock and MouseBind components
            // need to convert mouse position in screen camera to world position
            ScreenCamera camera = ecs.GetComponents<ScreenCamera>().First();
            if (ecs.GetComponents<ScreenCamera>().Count() != 1)
                throw new Exception("more than one screen camera!");

            Vector2 worldPoint = CameraRendering.ConvertScreenPointToWorldPoint(camera, newMouseState.Position, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

            // get all frames with mouse on it
            IEnumerable<Frame> worldFrames = from frame in ecs.GetComponents<Frame>()
                                             let transform = frame.Entity.GetComponent<Transform>()
                                             where new FloatRectangle(transform.Position.X, transform.Position.Y, frame.Width, frame.Height).ContainsPoint(worldPoint)
                                             orderby transform.Depth descending
                                             select frame;
            foreach (Frame frame in worldFrames)
            {
                InvokeMouseBindings(ecs, frame, ref foundBlock, newMouseState);

                if (foundBlock)
                    break;
            }

            OldMouseState = newMouseState;
        }

        private void InvokeMouseBindings(Ecs ecs, ComponentBase component, ref bool foundBlock, MouseState newMouseState)
        {
            if (component.Entity.HasComponent<MouseBind>())
            {
                MouseBind bind = component.Entity.GetComponent<MouseBind>();
                foreach ((UIMouseEvent mouseEvent, InputEvent? Event) in bind.Events)
                {
                    if (IsCurrentMouseEvent(mouseEvent, newMouseState) && Event is not null)
                    {
                        Event.InvokeEvent(ecs);
                    }
                }
            }
            if (component.Entity.HasComponent<MouseOverSensitivity>())
            {
                MouseOverSensitivity mouseOver = component.Entity.GetComponent<MouseOverSensitivity>();
                mouseOver.HasMouseOver = true;
            }

            if (component.Entity.HasComponent<MouseBlock>())
                foundBlock = true;
        }

        private IEnumerable<(UIFrame Frame, UIDepth Depth)> GetFramesWithMouseOver(UITransform transform, UIDepth depth, Rectangle realFrame, Point virtualSize, Point realMousePosition)
        {
            // find all children, check if their real frames contain the realMousePosition
            foreach (Component<UITransform> child in transform.Children)
            {
                UITransform childTransform = child.GetComponent();
                // must hhave a frame and be active
                if (!childTransform.Entity.HasComponent<UIFrame>() || !childTransform.Active)
                    continue;
                UIFrame childFrame = childTransform.Entity.GetComponent<UIFrame>();
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

        private bool MouseOnScreen(MouseState mouse, GameManager game)
        {
            if (mouse.Position.X < 0 || mouse.Position.Y < 0)
                return false;
            if (mouse.Position.X >= game.Window.ClientBounds.Width || mouse.Position.Y >= game.Window.ClientBounds.Height)
                return false;
            return true;
        }
    }
}
