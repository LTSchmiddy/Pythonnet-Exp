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
    public abstract class PythonModuleMemberObjectBase<T, U> : ScriptableObject where T : PythonModuleMemberReferenceBase, new() where U : PythonModuleMemberObjectBase<T, U> {

        [SerializeField] private T reference = new T();

        public T Reference { get => (T)reference; set => reference = value; }
        public PyObject Py {
            get {
                return Reference.Py;
            }
        }

#if UNITY_EDITOR
        public static U EditorNew(PythonFile p_file, string p_className) {
            U retVal = ScriptableObject.CreateInstance<U>();
            retVal.EditorInit(p_file, p_className);
            return retVal;
        }

        public void EditorInit(PythonFile p_file, string p_className) {
            Reference.EditorSetPythonFile(p_file);
            Reference.MemberName = p_className;
        }
#endif
    }
}