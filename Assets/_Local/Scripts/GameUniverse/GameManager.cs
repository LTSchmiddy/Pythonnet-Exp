using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUniverse {
    public class GameManager : MonoBehaviour
    {

        private static GameManager _instance;

        public static GameManager Instance { 
            get {
                return _instance;
            }
            private set {
                _instance = value;
            }
        }


        // This serves as a sort of... entry point for the game.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void GameInit() {
            if (Instance != null) { 
                // Manager was already loaded. Why are we importing this?
                Debug.Log("GameManager already loaded.");
                return;
            }
            Debug.Log("Loading GameManager...");

            // Load the Python Runtime:
            PythonScripting.PythonManager.Initialize();
            
            // Create Manager Object:
            GameObject go = new GameObject("GameManager");
            Instance = go.AddComponent<GameManager>();
            DontDestroyOnLoad(go);
            Instance.InstanceInit();
        }

        protected void InstanceInit() {

            
        }

        // Start is called before the first frame update
    }
}