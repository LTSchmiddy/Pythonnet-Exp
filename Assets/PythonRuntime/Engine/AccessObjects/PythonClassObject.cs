using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Python.Runtime;
using Object = UnityEngine.Object;
using PythonEngine.References;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif

namespace PythonEngine {
    [Serializable]
    public class PythonClassObject : PythonModuleMemberObjectBase<PythonClassReference, PythonClassObject> {

        public PyObject NewInstance(params object[] args) {
            return Reference.NewInstance(args);
        }
    }
}