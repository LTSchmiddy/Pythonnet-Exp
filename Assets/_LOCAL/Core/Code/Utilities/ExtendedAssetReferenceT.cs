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
    public class ExtendedAssetReferenceT<T> : AssetReferenceT<T> where T : Object {
        public ExtendedAssetReferenceT(string guid) : base(guid) { }

        public T AssetT {
            get => (T) Asset;
        }

        public T AssetSync {
            get {
                if (Asset == null){
                    var loadOp = LoadAssetAsync();
                    loadOp.WaitForCompletion();
                }

                return AssetT;
            }
        }

        public void LoadAssetSync(){
            LoadAssetSync<T>();
        }

        public void LoadAssetSync<U>(){
            var loadOp = LoadAssetAsync<U>();
            loadOp.WaitForCompletion();
        }

    }
}