using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif


namespace Utilities {
    [Serializable]
    public class AR_SceneAsset : AssetReferenceT<Object> {
        public AR_SceneAsset(string guid) : base(guid) { }

#if UNITY_EDITOR
        public override bool ValidateAsset(string path) {
            Object asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            return asset is SceneAsset;
        }
#endif
    }
}