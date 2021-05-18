using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif

namespace PythonEngine {
    [Serializable]
    public class PythonModuleReference : ISerializationCallbackReceiver {
#if UNITY_EDITOR
        // What we use in editor to select the scene
        [SerializeField] private Object moduleAsset;
        private bool IsValidSceneAsset {
            get {
                if (!moduleAsset) return false;
                return true;
                // return moduleAsset is TextAsset;
            }
        }
#endif

        // This should only ever be set during serialization/deserialization!
        [SerializeField]
        private string modulePath = string.Empty;

        // Use this when you want to actually have the scene path
        public string ModulePath {
            get {
#if UNITY_EDITOR
                // In editor we always use the asset's path
                return GetModulePathFromAsset();
#else
            // At runtime we rely on the stored path value which we assume was serialized correctly at build time.
            // See OnBeforeSerialize and OnAfterDeserialize
            return scenePath;
#endif
            }
            set {
                modulePath = value;
#if UNITY_EDITOR
                moduleAsset = GetModuleAssetFromPath();
#endif
            }
        }

        public string QualifiedModuleName {
            get {
                string retVal = ModulePath;
                retVal = retVal.Substring(0, retVal.Length - Path.GetExtension(retVal).Length);
                
                if (retVal.EndsWith("__init__")){
                    retVal = Path.GetDirectoryName(retVal);
                }
                
                // return retVal.Replace("\\", "/").Replace("/", ".");
                retVal = retVal.Replace("\\", "/").Replace("/", ".").Substring("Assets.".Length);
                return retVal;
            }
        }

        public PyObject Import() {
            using (Py.GIL()) {
                PythonManager.Scope.Import(QualifiedModuleName);
                return PythonManager.Scope.Get(QualifiedModuleName);
            }
        }

        public static implicit operator string(PythonModuleReference pyRef) {
            return pyRef.QualifiedModuleName;
        }

        // Called to prepare this data for serialization. Stubbed out when not in editor.
        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            HandleBeforeSerialize();
#endif
        }

        // Called to set up data for deserialization. Stubbed out when not in editor.
        public void OnAfterDeserialize() {
#if UNITY_EDITOR
            // We sadly cannot touch assetdatabase during serialization, so defer by a bit.
            EditorApplication.update += HandleAfterDeserialize;
#endif
        }



#if UNITY_EDITOR
        private SceneAsset GetModuleAssetFromPath() {
            return string.IsNullOrEmpty(modulePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(modulePath);
        }

        private string GetModulePathFromAsset() {
            return moduleAsset == null ? string.Empty : AssetDatabase.GetAssetPath(moduleAsset);
        }

        private void HandleBeforeSerialize() {
            // Asset is invalid but have Path to try and recover from
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(modulePath) == false) {
                moduleAsset = GetModuleAssetFromPath();
                if (moduleAsset == null) modulePath = string.Empty;

                EditorSceneManager.MarkAllScenesDirty();
            }
            // Asset takes precendence and overwrites Path
            else {
                modulePath = GetModulePathFromAsset();
            }

            // Debug.Log(QualifiedModuleName);
        }

        private void HandleAfterDeserialize() {
            EditorApplication.update -= HandleAfterDeserialize;
            // Asset is valid, don't do anything - Path will always be set based on it when it matters
            if (IsValidSceneAsset) return;

            // Asset is invalid but have path to try and recover from
            if (string.IsNullOrEmpty(modulePath)) return;

            moduleAsset = GetModuleAssetFromPath();
            // No asset found, path was invalid. Make sure we don't carry over the old invalid path
            if (!moduleAsset) modulePath = string.Empty;

            if (!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();

            // Debug.Log(QualifiedModuleName);
        }
#endif
    }
}