using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public class Texture2DAsset : AssetBase
    {
        public Texture2D Texture2D { get; }

        internal Texture2DAsset(AssetID<Texture2DAsset> assetID, Texture2D texture) : base(assetID)
        {
            Texture2D = texture;
        }
    }

    internal class OwnedTexture2DAsset : OwnedAsset
    {
        public Texture2D? Texture2D { get; private set; }
        public AssetID<Texture2DAsset> AssetID { get; }

        private OwnedTexture2DAsset(Texture2D texture2D, AssetID<Texture2DAsset> assetID)
        {
            Texture2D = texture2D;
            AssetID = assetID;
        }

        internal static OwnedTexture2DAsset Load(AssetID<Texture2DAsset> assetID)
        {
            try
            {
                Texture2D texture = Texture2D.FromFile(AssetManager.GraphicsDevice, assetID.Path);
                return new OwnedTexture2DAsset(texture, assetID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AssetNotFoundException(assetID);
            }
        }

        internal override AssetBase GetAsset()
        {
            if (Texture2D is null)
                throw new AssetAlreadyUnloadedException(AssetID);
            return new  Texture2DAsset(AssetID, Texture2D);
        }

        internal override void Unload()
        {
            if(Texture2D is null)
                throw new AssetAlreadyUnloadedException(AssetID);
            Texture2D.Dispose();
            Texture2D = null;
        }
    }
}
