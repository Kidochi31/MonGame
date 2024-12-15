using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    public static class CameraRendering
    {
        public static DrawLayer GetCameraLayer(Camera camera, Rectangle destination, float depth)
        {
            return new DrawLayer(camera.RenderTarget, new Rectangle(0, 0, camera.VirtualWidth, camera.VirtualHeight), destination, depth);
        }

        public static void LoadCameraImage(Ecs ecs, Camera camera)
        {
            Transform cameraTransform = camera.Entity.GetComponent<Transform>();
            FloatRectangle cameraBounds = new FloatRectangle(cameraTransform.Position.X, cameraTransform.Position.Y, camera.Width, camera.Height);

            // find all textures where the frame is in the camera
            var validTextures = from texture in ecs.GetComponents<Texture>()
                                let frame = texture.Entity.GetComponent<Frame>()
                                where FrameInBounds(frame, cameraBounds)
                                select texture;

            // now need to draw textures to the camera's render target
            RenderTarget2D renderTarget = camera.RenderTarget;
            ecs.GameManager.GraphicsDevice.SetRenderTarget(renderTarget);
            ecs.GameManager.GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch sb = ecs.GameManager.SpriteBatch;
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap);
            foreach (Texture texture in validTextures)
            {
                float textureDepth = texture.Entity.GetComponent<Transform>().Depth;
                Texture2D texture2D = texture.Asset.Texture2D;
                Rectangle textureSource = new Rectangle(0, 0, texture2D.Width, texture2D.Width);
                Rectangle textureDestination = GetFrameScreenRectangle(texture.Entity.GetComponent<Frame>(), cameraBounds, new(camera.VirtualWidth, camera.VirtualHeight));
                sb.Draw(texture2D, textureDestination, textureSource, Color.White, 0, Vector2.Zero, SpriteEffects.None, textureDepth);
            }
            sb.End();
        }

        static bool FrameInBounds(Frame frame, FloatRectangle bounds)
        {
            Transform frameTransform = frame.Entity.GetComponent<Transform>();
            FloatRectangle frameBounds = new FloatRectangle(frameTransform.Position.X, frameTransform.Position.Y, frame.Width, frame.Height);
            return bounds.OverlapsRectangle(frameBounds);
        }

        public static Vector2 ConvertScreenPointToWorldPoint(ScreenCamera screenCamera, Point screenPoint, int screenWidth, int screenHeight)
        {
            Camera camera = screenCamera.Entity.GetComponent<Camera>();
            Transform transform = screenCamera.Entity.GetComponent<Transform>();
            Point screenPointRelativeToCentre = screenPoint - new Point(screenWidth / 2, screenHeight / 2);
            Vector2 worldPoint = new Vector2(screenPointRelativeToCentre.X, screenPointRelativeToCentre.Y) / screenCamera.WorldToScreenScale + transform.Position;
            return worldPoint;
        }

        static Rectangle GetFrameScreenRectangle(Frame frame, FloatRectangle cameraBounds, Point screenSize)
        {
            Transform frameTransform = frame.Entity.GetComponent<Transform>();
            FloatRectangle frameBounds = new FloatRectangle(frameTransform.Position.X, frameTransform.Position.Y, frame.Width, frame.Height);

            Vector2 topLeft = frameBounds.TopLeft;
            Vector2 bottomRight = frameBounds.BottomRight;

            return new Rectangle(GetPositionScreenPoint(topLeft, cameraBounds, screenSize),
                                 GetPositionScreenPoint(bottomRight, cameraBounds, screenSize) - GetPositionScreenPoint(topLeft, cameraBounds, screenSize));
        }

        static Point GetPositionScreenPoint(Vector2 point, FloatRectangle cameraBounds, Point screenSize)
        {
            return new Point((int)((point.X - cameraBounds.Left) / cameraBounds.Width * screenSize.X),
                             (int)((point.Y - cameraBounds.Top) / cameraBounds.Height * screenSize.Y));
        }

        public static float ConvertWorldLengthToScreenLength(float worldLength, float worldToScreenScale)
        {
            return worldLength * worldToScreenScale;
        }

        public static float ConvertScreenLengthToWorldLength(float screenLength, float screenToScreenScale)
        {
            return screenLength / screenToScreenScale;
        }
    }
}
