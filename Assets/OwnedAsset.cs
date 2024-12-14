using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    internal abstract class OwnedAsset()
    {
        internal static OwnedAsset Load<T>(AssetID<T> assetID) where T: AssetBase
        {
            if(assetID is AssetID<Texture2DAsset> texture2D)
            {
                return OwnedTexture2DAsset.Load(texture2D);
            }
            if (assetID is AssetID<SoundEffectAsset> soundEffect)
            {
                return OwnedSoundEffectAsset.Load(soundEffect);
            }
            throw new InvalidAssetTypeException(typeof(T));
        }

        internal abstract void Unload();
        internal abstract AssetBase GetAsset();
    }
}
