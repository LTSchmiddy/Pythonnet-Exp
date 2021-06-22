using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

        // Runtime Values:
        private static bool loadingScreenIsRunning = false;

        // Instance Fields:
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] public GameDB dB;
        [SerializeField] private MapData mapData;
        [SerializeField] private LoadSaveManager loadSaveManager;

        public DataScene inAudioScene;
        public DataScene mainMenuScene;
        public DataScene optionsMenuScene;

        public GameObject loadingScreenPrefab;
        public GameObject gameCameraPrefab;

        // Instance Properties:
        public GameObject loadingScreenGO { get; private set; }
        public Camera gameCamera { get; private set; }
        public EventSystem eventSystem; // {get; private set;}

        public static bool LoadingScreenIsRunning {
            get {
                return loadingScreenIsRunning;
            }
            private set {
                loadingScreenIsRunning = value;
                Instance.loadingScreenGO.SetActive(value);
            }
        }

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
            eventSystem = GetComponent<EventSystem>();

            SetupGameCamera();
            SetupLoadingScreen();

            settingsManager.GetSettingsFromFile();
            settingsManager.ApplyAll();

            // Let's start by loading the MapData:
            mapData.Init();

            Debug.Log("Loaded Map Data.");
            loadSaveManager.ConstructNewSave();
#if UNITY_EDITOR
#else
            // Debug.LogError("Showing Console");
            // Loading Game from the top:
            LoadingScreenOperation(optionsMenuScene.LoadAsync());
            LoadingScreenOperation(mainMenuScene.LoadAsync());
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

        public static Coroutine LoadingScreenOperation(IEnumerator routine) {
            return Instance.StartCoroutine(LoadingScreenCoroutine(routine));
        }
        
        public static Coroutine LoadingScreenOperation(AsyncOperationHandle operation) {
            return Instance.StartCoroutine(LoadingScreenAsyncOperationHandle(operation));
        }

        public static Coroutine LoadingScreenOperation(AsyncOperation operation) {
            return Instance.StartCoroutine(LoadingScreenAsyncOperation(operation));
        }

        // Effectively, this serves as a wrapper for the regular Coroutine system.
        private static IEnumerator LoadingScreenCoroutine(IEnumerator routine) {
            // If there's another loading screen process running, wait for it to end.
            // Should prevent issues with multiple simultaneous loading screen routines.
            if (LoadingScreenIsRunning) {
                yield return null;
            }
            LoadingScreenIsRunning = true;
            while (routine.MoveNext()) {
                yield return routine.Current;
            }
            LoadingScreenIsRunning = false;
        }

        // Loading addressable assets/scenes can also be used as Loading screen operation.
        private static IEnumerator LoadingScreenAsyncOperationHandle(AsyncOperationHandle operation) {
            // If there's another loading screen process running, wait for it to end.
            // Should prevent issues with multiple simultaneous loading screen routines.
            if (LoadingScreenIsRunning) {
                yield return null;
            }
            LoadingScreenIsRunning = true;
            while(!operation.IsDone) { yield return null; }
            
            LoadingScreenIsRunning = false;
        }

        private static IEnumerator LoadingScreenAsyncOperation(AsyncOperation operation) {
            // If there's another loading screen process running, wait for it to end.
            // Should prevent issues with multiple simultaneous loading screen routines.
            if (LoadingScreenIsRunning) {
                yield return null;
            }
            LoadingScreenIsRunning = true;
            while(!operation.isDone) { yield return null; }
            
            LoadingScreenIsRunning = false;
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
                settingsManager.ApplyAll();
            }
        }

        void SetupLoadingScreen() {
            loadingScreenGO = Instantiate(loadingScreenPrefab);
            loadingScreenGO.transform.parent = transform;
            loadingScreenGO.SetActive(false);
            // loadingScreenGO.
        }

        void SetupGameCamera() {
            gameCamera = Instantiate(gameCameraPrefab).GetComponent<Camera>();
            gameCamera.transform.parent = transform;
        }


        void OnDestroy() {
            settingsManager.WriteSettingsFile();
        }
        #endregion
        // Start is called before the first frame update
    }
}