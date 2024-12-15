using Microsoft.Xna.Framework;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [FirstProcessor]
    public class RenderCameras : DrawProcessor
    {
        public override DrawLayer?[] Draw(GameTime gameTime, Ecs ecs, GameManager game)
        {
            int screenHeight = game.Window.ClientBounds.Height;
            int screenWidth = game.Window.ClientBounds.Width;


            // Set the size of screen cameras to match the screen
            IEnumerable<(ScreenCamera screenCam, Camera cam)> cameras = from sc in ecs.GetComponents<ScreenCamera>() select (sc, sc.Entity.GetComponent<Camera>());
            foreach ((ScreenCamera sc, Camera camera) in cameras)
            {
                float cameraHeight = CameraRendering.ConvertScreenLengthToWorldLength(screenHeight, sc.WorldToScreenScale);
                float cameraWidth = CameraRendering.ConvertScreenLengthToWorldLength(screenWidth, sc.WorldToScreenScale);
                camera.Width = cameraWidth;
                camera.Height = cameraHeight;
            }

            // draw all cameras to their render textures
            foreach(Camera camera in ecs.GetComponents<Camera>())
            {
                CameraRendering.LoadCameraImage(ecs, camera);
            }

            return Array.Empty<DrawLayer?>();
        }
    }
}
