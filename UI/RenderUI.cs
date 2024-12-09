using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.Drawing;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MonGame.UI
{
    [LastProcessor]
    public class RenderUI : DrawProcessor
    {
        public override DrawLayer?[] Draw(GameTime gameTime, Ecs ecs, GameManager game)
        {
            return (from gui in ecs.GetComponents<Gui>() select RenderGUI(gui, gameTime, ecs, game)).ToArray();
        }

        DrawLayer? RenderGUI(Gui gui, GameTime gameTime, Ecs ecs, GameManager game)
        {
            // First get the UITransform and render target from the gui
            UITransform baseTransform = gui.Entity.GetComponent<UITransform>();

            return CreateChildrenLayers(baseTransform, gameTime, ecs, game);
        }

        DrawLayer? CreateChildrenLayers(UITransform transform, GameTime gameTime, Ecs ecs, GameManager game)
        {
            Point position = transform.Position;
            float depth = transform.Depth;
            // check if it has a frame or texture
            if (transform.Entity.HasComponent<Texture>())
            {
                Texture texture = transform.Entity.GetComponent<Texture>();
                // if it has a texture, just return it
                Texture2D texture2D = texture.Asset.Texture2D;
                Rectangle source = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
                Rectangle destination = new Rectangle(position.X, position.Y, texture.Width, texture.Height);
                return new(texture2D, source, destination, depth);
            }
            else if (transform.Entity.HasComponent<Frame>())
            {
                Frame frame = transform.Entity.GetComponent<Frame>();
                // Get all the child textures
                var layers = (from child in transform.Children
                             let layer = CreateChildrenLayers(child.GetComponent(), gameTime, ecs, game)
                             where layer is not null
                             select (DrawLayer)layer).ToList();

                // draw all of the children to the frame
                RenderTarget2D renderTarget = frame.RenderTarget;
                game.GraphicsDevice.SetRenderTarget(renderTarget);
                game.GraphicsDevice.Clear(Color.Red);

                RenderDrawLayers(layers, game.SpriteBatch);

                Rectangle source = new Rectangle(0, 0, frame.VirtualWidth, frame.VirtualHeight);
                Rectangle destination = new Rectangle(position.X, position.Y, frame.ActualWidth, frame.ActualHeight);
                return new(renderTarget, source, destination, depth);
            }
            else if (transform.Entity.HasComponent<Gui>())
            {
                Gui gui = transform.Entity.GetComponent<Gui>();
                // Get all the child textures
                var layers = (from child in transform.Children
                                    let layer = CreateChildrenLayers(child.GetComponent(), gameTime, ecs, game)
                                    where layer is not null
                                    select (DrawLayer)layer).ToList();

                // draw all of the children to the frame
                RenderTarget2D renderTarget = gui.RenderTarget;
                game.GraphicsDevice.SetRenderTarget(renderTarget);
                game.GraphicsDevice.Clear(Color.Blue);

                RenderDrawLayers(layers, game.SpriteBatch);

                Rectangle source = new Rectangle(0, 0, gui.VirtualWidth, gui.VirtualHeight);
                Rectangle destination = new Rectangle(position.X, position.Y, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
                return new(renderTarget, source, destination, depth);
            }
            // otherwise return null
            return null;
        }

        void RenderDrawLayers(List<DrawLayer> layers, SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap);
            layers.ForEach(layer => layer.Draw(batch));
            batch.End();
        }

    }
}
