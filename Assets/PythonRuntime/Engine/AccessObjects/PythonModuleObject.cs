using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using PythonEngine.References;

namespace PythonEngine {
    // Originally, I was gonna use this scriptable object to store module paths
    // for Python scripts. However, the qualified module name wasn't being updated if the file was moved.
    // Ergo, we're just using this to ensure that Unity sees Python files as a unique type of asset.
    public class PythonModuleObject : ScriptableObject {
        [SerializeField] private PythonModuleReference reference = new PythonModuleReference();
        public List<PythonClassObject> classes = new List<PythonClassObject>();
        public List<PythonFunctionObject> functions = new List<PythonFunctionObject>();

        public PythonModuleReference Reference { get => reference; private set => reference = value; }
        public PyObject Module { 
            get => Reference.Module;
        }

        public PyObject LoadModule() {
            return Reference.GetModule();
        }

#if UNITY_EDITOR
        public static PythonModuleObject EditorNew(PythonFile p_file) {
            PythonModuleObject retVal = PythonModuleObject.CreateInstance<PythonModuleObject>();
            retVal.EditorInit(p_file);
            return retVal;
        }

        public void EditorInit(PythonFile p_file) {
            Reference.EditorSetPythonFile(p_file);
        }
#endif
    }
}