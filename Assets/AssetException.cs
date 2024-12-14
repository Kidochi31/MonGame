using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public abstract class AssetException(string message) : Exception(message) { }

    public class InvalidFileExtensionException(string message) : AssetException(message) { }
    public class InvalidAssetTypeException(Type type) : AssetException($"Type {type} is not a valid asset type!") { }
    public class AssetAlreadyUnloadedException(IAssetID assetID) : AssetException($"Asset {assetID} has already been unloaded!") { }
    public class AssetNotFoundException(IAssetID assetID) : AssetException($"Asset {assetID} cannot be found!") { }
    public class IncorrectAssetTypeException(IAssetID assetID, Type expected, Type actual) : AssetException($"Asset {assetID} was expected to be of type {expected} but was actually of type {actual}!") { }
}
