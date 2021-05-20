using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameUniverse.SceneTypes;
using LoadSave;
using GameMap;
using PythonEngine;

namespace GameUniverse {
    public class GlobalManager : MonoBehaviour
    {
        public const string GLOBAL_MANAGER_PREFAB_PATH = "_LOCAL/GlobalManager";

        public static GameObject PrefabGO {
            get => Resources.Load<GameObject>(GLOBAL_MANAGER_PREFAB_PATH);
        }
        public static GlobalManager Prefab {
            get => PrefabGO.GetComponent<GlobalManager>();
        }

        // Static values:
        private static GlobalManager _instance;
        public static GlobalManager Instance { get => _instance; private set => _instance = value; }
        
        // Properties:
        public GameDB dB;
        public MapData mapData;
        public LoadSaveManager loadSaveManager;
        public DataScene inGameDataScene;
        public DataScene inAudioScene;
        public DataScene mainMenuScene;

        

        // This serves as a sort of... entry point for the game.
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void GameInit() {
            if (Instance != null) { 
                // Manager was already loaded. Why are we importing this?
                Debug.Log("GameManager already loaded.");
                return;
            }
            Debug.Log("Loading GameManager Script...");

            // Load the Python Runtime:
            // PythonEngine.PythonManager.Initialize();
            PythonEngine.PythonManager.Reinitialize();

            // Create Manager Object:
            Instance = GameObject.Instantiate(PrefabGO).GetComponent<GlobalManager>();
            DontDestroyOnLoad(Instance.gameObject);
            Instance.InstanceInit();
        }
        protected void InstanceInit() {
            loadSaveManager = ScriptableObject.Instantiate<LoadSaveManager>(LoadSaveManager.Prefab);
            // mapData = ScriptableObject.Instantiate<MapData>(MapData.Prefab);
            mapData = MapData.Prefab;
        }

        void Start() {
            if (!inAudioScene.isLoaded) {                
                inAudioScene.LoadSync();
            }
        }

      // Start is called before the first frame update
    }
} 