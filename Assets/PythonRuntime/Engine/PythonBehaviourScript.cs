using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using RoboRyanTron.QuickButtons;

using LoadSave;
using Python.Runtime;

namespace PythonEngine {
    public class PythonBehaviourScript : MonoBehaviour, LoadSave.ISaveableBehaviour
    {
        public const string PYTHON_CLASS_INSTANCE_KEY = "pythonClassInstance";
        public ScriptTypes.PythonClassInstance pythonClassInstance;

        public void LinkSaveableInfo(Dictionary<string, ISaveableInfo> info) {
            if (info.ContainsKey(PYTHON_CLASS_INSTANCE_KEY)) {
                pythonClassInstance = (ScriptTypes.PythonClassInstance)info[PYTHON_CLASS_INSTANCE_KEY];
            } else {
                pythonClassInstance.Construct();
                info.Add(PYTHON_CLASS_INSTANCE_KEY, pythonClassInstance);
            }
        }



        // Start is called before the first frame update
        void Awake () {}

        void Start() {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
