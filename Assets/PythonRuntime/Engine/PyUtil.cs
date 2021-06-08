using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Python.Runtime;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PythonEngine {

    // Python Uti
    public static class PyUtil {
        
        /// <summary>
        /// Gets a Python module, importing it if necessary.
        /// </summary>
        public static PyObject GetModule(string name) {
            return PythonManager.GetModule(name);
        }

        /// <summary>
        /// Can be used to call Python functions or construct new Python objects.
        /// </summary>
        public static PyObject Invoke(string modulePath, string memberName, params PyObject[] args) {
            return PythonManager.Invoke(modulePath, memberName, args);
        }
    }
}