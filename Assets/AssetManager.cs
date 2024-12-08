using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public static class AssetManager
    {
        // Path -> (ReferenceCount, Asset)
        static Dictionary<string, (int ReferenceCount, OwnedAsset Asset)> Assets = [];
        public static GraphicsDevice GraphicsDevice;

        public static void Initialise(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        internal static void UnloadAsset(IAssetID id)
        {
            string path = id.Path;
            if (!Assets.ContainsKey(path))
                throw new AssetAlreadyUnloadedException(id);
            // the asset is there -> decrease the reference count
            (int referenceCount, OwnedAsset asset) = Assets[path];
            referenceCount -= 1;
            if(referenceCount <= 0)
            {
                Assets.Remove(path);
                asset.Unload();
            }
            else
            {
                Assets[path] = (referenceCount, asset);
            }
        }

        public static T LoadAsset<T>(AssetID<T> id) where T : AssetBase
        {
            string path = id.Path;
            int referenceCount;
            OwnedAsset asset;
            if (!Assets.ContainsKey(path))
            {
                // load asset
                asset = OwnedAsset.Load(id);
                referenceCount = 1;
            }
            else
            {
                // add to reference count and return asset
                (referenceCount, asset) = Assets[path];
                referenceCount += 1;
            }
            Assets[path] = (referenceCount, asset);
            AssetBase newAsset = asset.GetAsset();
            if (newAsset is T)
                return (T)newAsset;
            throw new IncorrectAssetTypeException(id, typeof(T), newAsset.GetType());
        }
    }
}
