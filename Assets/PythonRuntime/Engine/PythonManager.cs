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
    public static class PythonManager {

        public const string PYTHON_RUNTIME_DIRECTORY = "./PythonRuntime/";

        private static PyScope scope;

        public static PyScope Scope {
            get {
                return scope;
            }
            private set {
                scope = value;
            }
        }

        public static bool IsInitialized {
            get => Python.Runtime.PythonEngine.IsInitialized;
        }

        private static PyObject pickle;
        public static PyObject Pickle { get => pickle; private set => pickle = value; }

#if UNITY_EDITOR
        private static PyObject quickDebug;
        public static PyObject QuickDebug { get => quickDebug; private set => quickDebug = value; }
#endif
        public static void Initialize() {
            if (Python.Runtime.PythonEngine.IsInitialized) {
                return;
            }

            Python.Runtime.PythonEngine.PythonHome = PYTHON_RUNTIME_DIRECTORY + "python-3.9.4-embed-amd64";
            Python.Runtime.PythonEngine.Initialize();
            Scope = Py.CreateScope();
            using (Py.GIL()) {
                Scope.Import("unity_py");
                Pickle = GetModule("unity_pickler");

#if UNITY_EDITOR
                QuickDebug = GetModule("unity_editor_quick_python_debugging");
                QuickDebug.InvokeMethod("start_python_debugging");
#endif
            }
        }

        public static void Uninitialize() {
            if (!Python.Runtime.PythonEngine.IsInitialized) {
                // Debug.Log("Python is already shutdown...");
                return;
            }
            // Debug.Log("Shutting down Python...");
            Python.Runtime.PythonEngine.Shutdown();
            // Scope.Dispose();
            Scope = null;
        }

        public static string GetQualifiedModuleName(string modulePath) {
            string retVal = modulePath;
            retVal = retVal.Substring(0, retVal.Length - Path.GetExtension(retVal).Length);

            if (retVal.EndsWith("__init__")) {
                retVal = Path.GetDirectoryName(retVal);
            }

            // return retVal.Replace("\\", "/").Replace("/", ".");
            retVal = retVal.Replace("\\", "/").Replace("/", ".").Substring("Assets.".Length);
            return retVal;
        }

#if UNITY_EDITOR
        [MenuItem("Python/Reload Python")]
#endif
        public static void Reinitialize() {
            Uninitialize();
            Initialize();
        }

        /// <summary>
        /// Gets a Python module, importing it if necessary.
        /// </summary>
        public static PyObject GetModule(string moduleName) {
            using (Py.GIL()) {
                Scope.Import(moduleName);
                return Scope.Get(moduleName);
            }
        }

        /// <summary>
        /// Can be used to call Python functions or construct new Python objects.
        /// </summary>
        public static PyObject Invoke(string modulePath, string memberName, params PyObject[] args) {
            PyObject module = GetModule(modulePath);
            using (Py.GIL()) {
                return module.InvokeMethod(memberName, args);
            }
        }

        // Python Serialization Methods:
        public static byte[] PickleData(PyObject obj) {
            using (Py.GIL()) {
                return Pickle.InvokeMethod("dumps", obj).As<byte[]>();
            }
        }

        public static PyObject UnpickleData(byte[] data) {
            using (Py.GIL()) {
                return Pickle.InvokeMethod("loads", data.ToPython());

            }
        }

        public static void UnpickleDataInto(byte[] data, PyObject obj) {
            using (Py.GIL()) {
                Pickle.InvokeMethod("loads_into", data.ToPython(), obj);

            }
        }
    }

    public class PickledPyObject {
        public string module;
        public byte[] data;


        public PickledPyObject() { }
        public PickledPyObject(PyObject obj) {
            Pickle(obj);
        }
        public PickledPyObject(string p_module, byte[] p_data) {
            module = p_module;
            data = p_data;
        }

        public void Pickle(PyObject obj) {
            using (Py.GIL()) {
                module = PythonManager.Pickle.InvokeMethod("get_module", obj).As<string>();
                data = PythonManager.PickleData(obj);

            }
        }

        public PyObject Unpickle() {
            using (Py.GIL()) {
                PythonManager.Scope.Import(module);
                return PythonManager.UnpickleData(data);
            }
        }

        public void UnpickleInto(PyObject obj) {
            using (Py.GIL()) {
                PythonManager.Scope.Import(module);
                PythonManager.UnpickleDataInto(data, obj);
            }
        }
    }
}
