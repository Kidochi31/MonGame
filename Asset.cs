using MonGame.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame
{
    public static class Asset
    {
        public static AssetID<Texture2DAsset> Birb = new("Birb", "birb.jpg");
    }
}
