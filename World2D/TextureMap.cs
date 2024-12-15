using MonGame.Assets;
using MonGame.ECS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.World2D
{
    [RequiresComponent(typeof(Transform))]
    [RequiresComponent(typeof(Frame))]
    public sealed record class TextureMap(int XCells, int YCells, Entity Entity) : ComponentBase(Entity)
    {
        public int XCells { get; } = XCells;
        public int YCells { get; } = YCells;

        public Texture2DAsset?[,] Textures { get; set; } = new Texture2DAsset?[XCells, YCells]; 
    }
}
