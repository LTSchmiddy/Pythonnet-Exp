using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LoadSave {
    [CreateAssetMenu(menuName="_LOCAL/LoadSave/LoadSaveData Object", fileName="LoadSaveDataObject", order=0)]
    public class LoadSaveManager : ScriptableObject
    {
        public const string GLOBAL_MANAGER_PREFAB_PATH = "_LOCAL/LoadSaveDataObject";
        // Start is called before the first frame update
        public static LoadSaveManager Prefab {
            get => Resources.Load<LoadSaveManager>(GLOBAL_MANAGER_PREFAB_PATH);
        }
        public static LoadSaveManager Main {
            get {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    return Prefab;
                }
#endif
                if (GlobalManager.Instance == null) {
                    return null;
                }
                return GlobalManager.Instance.loadSaveManager;
            }
        }

        public SaveFile data = new SaveFile();

        

    }
}
