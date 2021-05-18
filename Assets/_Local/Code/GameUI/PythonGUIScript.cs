using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PythonEngine;
using Python.Runtime;

namespace GameUI {
    public class PythonGUIScript : MonoBehaviour
    {
        // Start is called before the first frame update

        public PythonModuleReference script;

        private PyObject _module;
        void Start() {
            _module = script.Import();
        }

        // Update is called once per frame
        void OnGUI() {
            _module.InvokeMethod("OnGUI", gameObject.ToPython());
        }
    }

}
