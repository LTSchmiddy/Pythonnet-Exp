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

        public PythonFunctionObject function;

        // Update is called once per frame
        void OnGUI() {
            function.Call(gameObject);
        }
    }

}
