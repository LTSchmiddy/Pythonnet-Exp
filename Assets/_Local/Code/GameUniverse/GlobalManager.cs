using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GameUniverse.SceneTypes;

namespace GameUniverse {
    public class GlobalManager : MonoBehaviour
    {
        public const string GLOBAL_MANAGER_PREFAB_PATH = "_LOCAL/GlobalManager";
        

        // Static values:
        private static GlobalManager _instance;
        public static GlobalManager Instance { get => _instance; private set => _instance = value; }
        
        // Properties:
        public GameDB dB;

        public DataScene inGameDataScene;
        public DataScene inAudioScene;


        // static GameManager() {

        // }

        // This serves as a sort of... entry point for the game.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void GameInit() {
            if (Instance != null) { 
                // Manager was already loaded. Why are we importing this?
                Debug.Log("GameManager already loaded.");
                return;
            }
            Debug.Log("Loading GameManager Script...");

            // Load the Python Runtime:
            PythonEngine.PythonManager.Initialize();

            // Create Manager Object:
            Instance = GameObject.Instantiate(Resources.Load<GameObject>(GLOBAL_MANAGER_PREFAB_PATH)).GetComponent<GlobalManager>();
            DontDestroyOnLoad(Instance.gameObject);
            Instance.InstanceInit();
        }

        public static bool IsSceneLoaded(SceneReference sceneRef) {
            return SceneManager.GetSceneByPath(sceneRef.ScenePath).isLoaded;
        }

        protected void InstanceInit() {
            if (!inAudioScene.isLoaded){
                new LoadSceneParameters();
                
                inAudioScene.Load();
            }
            
        }

        // Start is called before the first frame update
    }
}