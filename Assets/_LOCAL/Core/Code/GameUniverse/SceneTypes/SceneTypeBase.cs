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
    /// An extension of the SceneReference class, providing some scene management convenience functions.
    /// </summary>
    [Serializable]
    public class SceneTypeBase : SceneReference {

        
        public Scene scene {
            get {
                return SceneManager.GetSceneByPath(ScenePath);
            }
        }

        public bool isLoaded {
            get {
                return SceneManager.GetSceneByPath(ScenePath).isLoaded;
            }
        }

        public virtual Scene LoadSync(LoadSceneParameters mode) {
            return SceneManager.LoadScene(ScenePath, mode);
        }

        public virtual AsyncOperation LoadAsync(LoadSceneParameters mode) {
            return SceneManager.LoadSceneAsync(ScenePath, mode);
        }

        public virtual AsyncOperation UnloadAsync() {
            return SceneManager.UnloadSceneAsync(scene);
        }

        #if UNITY_EDITOR
        public void EditorClose(bool removeScene) {
            EditorSceneManager.CloseScene(scene, removeScene);
        }
        #endif

    }

}