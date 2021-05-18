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
        static PyScope scope { get { return PythonManager.Scope; } }

        
        public PythonModuleReference moduleRef;
        // public string moduleRef;
        public string className;
        public PyObject instance;

        
        public void Construct() {
                instance = PythonManager.CreateInstance(moduleRef.QualifiedModuleName, className);
                // instance = PythonManager.CreateInstance(moduleRef, className);
        }
    }
}
