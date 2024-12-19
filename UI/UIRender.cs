using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.Drawing;
using MonGame.ECS;
using MonGame.World2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MonGame.UI
{
    [LastProcessor]
    public class UIRender : DrawProcessor
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
            //only draw active layers
            if (!transform.Active)
                return null;

            Point position = transform.Position;
            float depth = transform.Depth;
            // add color if necessary
            ColorF? color = null;
            if (transform.Entity.HasComponent<UIColor>())
                color = transform.Entity.GetComponent<UIColor>().Color;

            // check if it has a frame or Gui
            if (transform.Entity.HasComponent<UIFrame>())
            {
                UIFrame frame = transform.Entity.GetComponent<UIFrame>();
                
                
                // Get all the child textures
                List<DrawLayer> Layers = [];
                // if this entity also has a texture, then add it to the list (at the very back)
                if (transform.Entity.HasComponent<UITexture>())
                {
                    UITexture texture = transform.Entity.GetComponent<UITexture>();
                    // Add the texture to the list
                    Texture2D texture2D = texture.Asset.Texture2D;
                    Rectangle textureSource = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
                    Rectangle textureDestination = new Rectangle(0, 0, frame.VirtualWidth, frame.VirtualHeight);
                    Layers.Add(new(texture2D, textureSource, textureDestination, 0));
                }
                Layers.AddRange(from child in transform.Children
                             let layer = CreateChildrenLayers(child.GetComponent(), gameTime, ecs, game)
                             where layer is not null
                             select (DrawLayer)layer);
                

                // draw all of the children to the frame
                RenderTarget2D renderTarget = frame.RenderTarget;
                game.GraphicsDevice.SetRenderTarget(renderTarget);
                game.GraphicsDevice.Clear(Color.Transparent);

                RenderDrawLayers(Layers, game.SpriteBatch);

                Rectangle source = new Rectangle(0, 0, frame.VirtualWidth, frame.VirtualHeight);
                Rectangle destination = new Rectangle(position.X, position.Y, frame.ActualWidth, frame.ActualHeight);

                return new(renderTarget, source, destination, depth, color);
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
                game.GraphicsDevice.Clear(Color.Transparent);

                RenderDrawLayers(layers, game.SpriteBatch);

                Rectangle source = new Rectangle(0, 0, gui.VirtualWidth, gui.VirtualHeight);
                Rectangle destination = new Rectangle(position.X, position.Y, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

                return new(renderTarget, source, destination, depth, color);
            }
            // otherwise return null
            return null;
        }

        void RenderDrawLayers(List<DrawLayer> layers, SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap);
            layers.ForEach(layer => layer.Draw(batch));
            batch.End();
        }

    }
}
