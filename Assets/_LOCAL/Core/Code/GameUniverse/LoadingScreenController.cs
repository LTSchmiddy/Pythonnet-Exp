using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUniverse {
    public class LoadingScreenController : MonoBehaviour {
        private static LoadingScreenController _instance;
        public static LoadingScreenController Instance { get => _instance; private set => _instance = value; }

        void Awake () {
            Instance = this;
        }

    }
}