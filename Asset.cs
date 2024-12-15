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
        public static AssetID<Texture2DAsset> Birb2 = new("Birb2", "birb2.jpg");
        public static AssetID<SoundEffectAsset> Secret = new("Secret", "secret.ogg");
        public static AssetID<Texture2DAsset> Birb3 = new("Birb3", "birb3.png");
        public static AssetID<Texture2DAsset> Birb4 = new("Birb4", "birb4.png");
    }
}
