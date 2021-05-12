using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Python.Runtime;

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

        public static PyObject Pickle { get => pickle; private set => pickle = value; }

        private static PyObject pickle;

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize() {
            if (Python.Runtime.PythonEngine.IsInitialized) {
                Debug.Log("Python is already Initialized...");
                return;
            }

            Debug.Log("Initializing Python...");

            Python.Runtime.PythonEngine.PythonHome = "./PythonRuntime/python-3.9.4-embed-amd64";
            Python.Runtime.PythonEngine.Initialize();
            Scope = Py.CreateScope();
            using (Py.GIL()) {
                Scope.Import("unity_py_loader");
                Scope.Import("unity_pickler");
                Pickle = Scope.Get("unity_pickler");    
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
