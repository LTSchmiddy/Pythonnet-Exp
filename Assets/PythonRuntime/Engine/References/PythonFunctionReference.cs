using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif
using PythonEngine;

namespace PythonEngine.References {
    [Serializable]
    public class PythonFunctionReference : PythonModuleMemberReferenceBase {

        public PyObject Call(params object[] args) {
            return TranslateInvoke(args);
        }
    }
}