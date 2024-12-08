using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public abstract class AssetBase(IAssetID assetID)
    {
        readonly IAssetID AssetID = assetID;

        ~AssetBase()
        {
            AssetManager.UnloadAsset(AssetID);
        }
    }
}
