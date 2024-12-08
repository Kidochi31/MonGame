using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public interface IAssetID
    {
        public string Name { get; }
        public string Path { get; }
    }

    public readonly record struct AssetID<T>(string Name, string Path) : IAssetID where T : AssetBase
    {
        public T Load()
        {
            return AssetManager.LoadAsset(this);
        }
    }
}
