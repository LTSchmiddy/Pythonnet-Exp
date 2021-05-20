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

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize() {
            if (Python.Runtime.PythonEngine.IsInitialized) {
                // Debug.Log("Python is already initialized...");
                return;
            }

            // Debug.Log("Initializing Python...");

            Python.Runtime.PythonEngine.PythonHome = "./PythonRuntime/python-3.9.4-embed-amd64";
            Python.Runtime.PythonEngine.Initialize();
            Scope = Py.CreateScope();
            using (Py.GIL()) {
                Scope.Import("unity_py_loader");

                Pickle = GetModule("unity_pickler"); 
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
            
            if (retVal.EndsWith("__init__")){
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

        
        // Python Code Access:
        public static PyObject GetModule(string moduleName) {
            using (Py.GIL()) {
                Scope.Import(moduleName);
                return Scope.Get(moduleName);
            }
        }

        public static PyObject CreateInstance(string modulePath, string className, params PyObject[] args) {
            using (Py.GIL()) {
                if (string.IsNullOrWhiteSpace(modulePath)) {
                    modulePath = "builtins";
                } else if (!scope.Contains(modulePath)) {
                    scope.Import(modulePath);
                }
                
                PyObject module = scope.Get(modulePath);

                return module.InvokeMethod(className, args);
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


        public PickledPyObject() {}
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
