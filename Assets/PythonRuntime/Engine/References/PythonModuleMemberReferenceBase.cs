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
    public abstract class PythonModuleMemberReferenceBase : PythonModuleReference {
        [SerializeField] private string memberName = "";
        public string MemberName { get => memberName; set => memberName = value; }

        private PyObject py;
        public PyObject Py {
            get {
                if (py == null) {
                    GetPyObject();
                }

                return py;
            }
        }

        public PyObject GetPyObject() {
            py = Module.GetAttr(MemberName);
            return py;
        }

        public PyObject TranslateInvoke(params object[] args) {
            PyObject[] pyArgs;
            if (args is PyObject[]) {
                pyArgs = (PyObject[])args;
            } else {
                pyArgs = new PyObject[args.Length];

                for (int i = 0; i < args.Length; i++) {
                    if (args[i] is PyObject) {
                        pyArgs[i] = (PyObject)args[i];
                    } else {
                        pyArgs[i] = args[i].ToPython();
                    }
                }
            }
            using (Python.Runtime.Py.GIL()) {
                return Py.Invoke(pyArgs);
            }
        }
    }
}