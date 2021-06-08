using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using GameUniverse;
using GameUniverse.SceneTypes;

using System.Linq;
using Object = UnityEngine.Object;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utilities;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif

namespace GameMap {
    /// <summary>
    /// SceneType used for storing game managers and data. Should be loaded additively.
    /// </summary>
    [Serializable]
    public class MapScene : AR_SceneAsset {
        

        public MapScene(string guid) : base(guid) { }

#if UNITY_EDITOR
        public Object GetSceneAsset() {
            return this.editorAsset;
        }
#endif


    }
}