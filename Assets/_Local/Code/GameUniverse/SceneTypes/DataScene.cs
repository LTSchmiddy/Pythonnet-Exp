using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using System.Linq;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif

namespace GameUniverse.SceneTypes {
    /// <summary>
    /// SceneType used for storing game managers and data. Should be loaded additively.
    /// </summary>
    [Serializable]
    public class DataScene : SceneTypeBase {
        public static LoadSceneParameters DATA_SCENE_MODE = new LoadSceneParameters(LoadSceneMode.Additive);

        public bool loadAsync = false;

        public AsyncOperation Load() {
            return LoadAsync(DATA_SCENE_MODE);
        }
    }
}