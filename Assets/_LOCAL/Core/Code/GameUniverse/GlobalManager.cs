using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

using GameUniverse.SceneTypes;
using LoadSave;
using GameMap;
using PythonEngine;
using GameSettings;
namespace GameUniverse {
    public class GlobalManager : MonoBehaviour {
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
        public static SettingsManager Settings { get => Instance.settingsManager; private set => Instance.settingsManager = value; }
        public static MapData Map { get => Instance.mapData; private set => Instance.mapData = value; }
        public static LoadSaveManager LoadSave { get => Instance.loadSaveManager; private set => Instance.loadSaveManager = value; }


        // Instance Fields:
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] public GameDB dB;
        [SerializeField] private MapData mapData;
        [SerializeField] private LoadSaveManager loadSaveManager;

        // Might ditch these... not sure yet.
        public DataScene inGameDataScene;
        public DataScene inAudioScene;
        public DataScene mainMenuScene;


        #region Startup
        // This serves as a sort of... entry point for the game.
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#else        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#endif
        public static void GameInit() {
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

            // Now that the instance was created, we can perform actions that require the controller's 
            // serialized data (stored values, references to ScriptableObjects, etc.)
            Instance.InstanceInit();
        }
        protected void InstanceInit() {
            settingsManager.GetSettingsFromFile();
            settingsManager.Apply();

            // Let's start by loading the MapData:
            mapData.Init();

            Debug.Log("Loaded Map Data.");
            loadSaveManager.ConstructNewSave();
#if UNITY_EDITOR
#else
            Debug.LogError("Showing Console");
#endif
        }

        #endregion


        #region Static Methods
        /// <summary>
        /// Need to invoke a coroutine from a non-MB object? Do it here.
        /// </summary>
        public static Coroutine GlobalCoroutine(IEnumerator routine) {
            return Instance.StartCoroutine(routine);
        }

        #region Game Loading
        public static void LoadNewGame() {
            LoadSave.ConstructNewSave();
            Map.GoToRoom(LoadSave.data.sceneId);
        }

        public static void LoadIntoGame() {
            Debug.Log("Not implemented yet...");
        }

        #endregion
        #endregion
        #region Instance Methods

        void Start() {
            if (!inAudioScene.isLoaded) {
                inAudioScene.LoadSync();
            }
        }

        void Update() {
            if (Keyboard.current[Key.Space].wasPressedThisFrame) {
                Debug.Log("Applying Settings...");
                settingsManager.GetSettingsFromFile();
                settingsManager.Apply();
            }
        }
        #endregion
        // Start is called before the first frame update
    }
}