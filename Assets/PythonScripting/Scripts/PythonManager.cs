using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Python.Runtime;

namespace PythonScripting {
    public static class PythonManager {

        private static PyScope masterScope;

        public static PyScope MasterScope {     
            get { 
                return masterScope;
            }
            set {
                masterScope = value;
            }
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize() { 
            PythonEngine.PythonHome = "./PythonRuntime/python-3.9.4-embed-amd64";
            PythonEngine.Initialize();
            MasterScope = Py.CreateScope();
            using (Py.GIL()) {
                MasterScope.Import("unity_py_loader");
            }
        }
    }
}
