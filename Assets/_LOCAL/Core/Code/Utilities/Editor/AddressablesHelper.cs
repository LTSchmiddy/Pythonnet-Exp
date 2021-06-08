using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.AddressableAssets;
using UnityEditor;


namespace UtilitiesEditor {
    public static class AddressablesHelper {
        // This assumes we're using the FilePath as the asset's path:
        public static string GetAddressablesAddress(this UnityEngine.Object asset) {
            AssetReference assetRef = new AssetReference();
            assetRef.SetEditorAsset(asset);
            string guid = assetRef.ToString();
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return path;
        }
    }
}
