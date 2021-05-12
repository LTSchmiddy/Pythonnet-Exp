using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Python.Runtime;
using PythonEngine;
using LoadSave;

namespace PythonEngine.ScriptTypes {

    [SerializableAttribute]
    public class PythonClassInstance : ISaveableInfo {
        PyScope scope { get { return PythonManager.Scope; } }

        public string modulePath;
        public string className;
        public PyObject instance;

        
        public void Construct() {
                instance = PythonManager.CreateInstance(modulePath, className);
        }
    }
}
