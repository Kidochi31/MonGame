using Microsoft.Xna.Framework;
using MonGame.ECS;
using MonGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [LastProcessor]
    [BeforeProcessor(typeof(UIRender))]
    public class RenderWorld : DrawProcessor
    {
        public override DrawLayer?[] Draw(GameTime gameTime, Ecs ecs, GameManager game)
        {
            int screenHeight = game.Window.ClientBounds.Height;
            int screenWidth = game.Window.ClientBounds.Width;

            // first set their cameras' widths and heights to the screen equivalent
            IEnumerable<Camera> cameras = from sc in ecs.GetComponents<ScreenCamera>() select sc.Entity.GetComponent<Camera>();

            // return the layers of each camera, using the depth of the camera's transform as its depth,
            // and the screen as the target size
            var layers = (from camera in cameras
                    select (DrawLayer?)CameraRendering.GetCameraLayer(camera, new(0, 0, screenWidth, screenHeight), camera.Entity.GetComponent<Transform>().Depth)).ToArray();

            return layers;
        }
    }
}
