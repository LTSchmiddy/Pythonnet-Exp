using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameUniverse;
using GameUniverse.SceneTypes;

using System.Linq;
using Object = UnityEngine.Object;
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
    public class MapScene : SceneTypeBase {
        public static LoadSceneParameters DATA_SCENE_MODE = new LoadSceneParameters(LoadSceneMode.Additive);

        public AsyncOperation LoadAsync() {
            return LoadAsync(DATA_SCENE_MODE);
        }

        public Scene LoadSync() {
            return LoadSync(DATA_SCENE_MODE);
        }
    }
}