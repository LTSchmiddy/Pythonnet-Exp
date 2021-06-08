using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using PythonEngine.References;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif


namespace PythonEngine {
    [Serializable]
    public class PythonFunctionObject : PythonModuleMemberObjectBase<PythonFunctionReference, PythonFunctionObject> {

        public PyObject Call(params object[] args) {
            return Reference.Call(args);
        }
    }
}