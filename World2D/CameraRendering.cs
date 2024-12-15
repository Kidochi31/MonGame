using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonGame.Assets;
using MonGame.ECS;
using MonGame.UI;
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
            var validTextures = GetTextures(cameraBounds, ecs, camera)
                        .Concat(GetTexturesInMaps(cameraBounds, ecs, camera));

            // now need to draw textures to the camera's render target
            RenderTarget2D renderTarget = camera.RenderTarget;
            ecs.GameManager.GraphicsDevice.SetRenderTarget(renderTarget);
            ecs.GameManager.GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch sb = ecs.GameManager.SpriteBatch;
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap);
            validTextures.ForEach(layer => layer.Draw(sb));
            sb.End();
        }

        static IEnumerable<DrawLayer> GetTextures(FloatRectangle cameraBounds, Ecs ecs, Camera camera)
        {
            // find all textures where the frame is in the camera
           return from texture in ecs.GetComponents<Texture>()
                  let frame = texture.Entity.GetComponent<Frame>()
                  where FrameInBounds(frame, cameraBounds)
                  let textureDepth = texture.Entity.GetComponent<Transform>().Depth
                  let texture2D = texture.Asset.Texture2D
                  let textureSource = new Rectangle(0, 0, texture2D.Width, texture2D.Height)
                  let textureDestination = GetFrameScreenRectangle(GetFrameBounds(texture.Entity.GetComponent<Frame>()), cameraBounds, new(camera.VirtualWidth, camera.VirtualHeight))
                  select new DrawLayer(texture2D, textureSource, textureDestination, textureDepth);
        }

        static IEnumerable<DrawLayer> GetTexturesInMaps(FloatRectangle cameraBounds, Ecs ecs, Camera camera)
        {
            var maps = ecs.GetComponents<TextureMap>();
            foreach(var map in maps)
            {

                Frame frame = map.Entity.GetComponent<Frame>();
                if (!FrameInBounds(frame, cameraBounds))
                    continue;
                var textureDepth = map.Entity.GetComponent<Transform>().Depth;
                var mapBounds = GetFrameBounds(frame);

                // go through all x and y cells
                for (int x = 0; x < map.XCells; x++)
                {
                    for (int y = 0; y < map.YCells; y++)
                    {
                        var bounds = GetTextureMapCellBounds(map, mapBounds, x, y);
                        if (cameraBounds.OverlapsRectangle(bounds))
                        {
                            var texture2D = map.Textures[x, y]?.Texture2D;
                            if (texture2D is null)
                                continue;
                            var textureSource = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
                            var textureDestination = GetFrameScreenRectangle(bounds, cameraBounds, new(camera.VirtualWidth, camera.VirtualHeight));
                            yield return new DrawLayer(texture2D, textureSource, textureDestination, textureDepth);
                        }
                    }
                }
            }
            yield break;
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

        static Rectangle GetFrameScreenRectangle(FloatRectangle frame, FloatRectangle cameraBounds, Point screenSize)
        {
            Vector2 topLeft = frame.TopLeft;
            Vector2 bottomRight = frame.BottomRight;

            return new Rectangle(GetPositionScreenPoint(topLeft, cameraBounds, screenSize),
                                 GetPositionScreenPoint(bottomRight, cameraBounds, screenSize) - GetPositionScreenPoint(topLeft, cameraBounds, screenSize));
        }

        static FloatRectangle GetFrameBounds(Frame frame)
        {
            Transform frameTransform = frame.Entity.GetComponent<Transform>();
            FloatRectangle frameBounds = new FloatRectangle(frameTransform.Position.X, frameTransform.Position.Y, frame.Width, frame.Height);
            return frameBounds;
        }

        static FloatRectangle GetTextureMapCellBounds(TextureMap map, FloatRectangle bounds, int xCell, int yCell)
        {
            Vector2 cellSize = new Vector2(bounds.Width / map.XCells, bounds.Height / map.YCells);
            Vector2 cellStart = new Vector2(cellSize.X * xCell, cellSize.Y * yCell) + bounds.TopLeft;
            Vector2 cellEnd = cellStart + cellSize;
            return new FloatRectangle(cellStart, cellEnd);
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
