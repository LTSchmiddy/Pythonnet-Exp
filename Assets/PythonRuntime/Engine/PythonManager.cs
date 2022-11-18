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

        public const string PYTHON_SCRIPT_BUNDLE_DIRECTORY = "ScriptBundles";
        public const string PYTHON_RUNTIME_DIRECTORY = "./PythonInterpreters/";


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

        private static dynamic unityPy;

        private static PyObject pickleModule;
        public static PyObject PickleModule { get => pickleModule; private set => pickleModule = value; }
        public static dynamic UnityPy { get => unityPy; private set => unityPy = value; }
        public static string GetPythonHome() {
            #if UNITY_EDITOR
                return PYTHON_RUNTIME_DIRECTORY + "python-3.9.4-embed-amd64";
            #else
                return "./PythonRuntime";
            #endif
        }

        public static string GetPythonScriptBundleDirectory() {
            return Application.streamingAssetsPath + "/" + PYTHON_SCRIPT_BUNDLE_DIRECTORY;
        }

        public static void Initialize() {
            if (Python.Runtime.PythonEngine.IsInitialized) {
                return;
            }

            Python.Runtime.PythonEngine.PythonHome = GetPythonHome();
            Python.Runtime.PythonEngine.Initialize();
            Scope = Py.CreateScope();
            ConfigureSys();
            using (Py.GIL()) {
                UnityPy = Scope.Import("unity_py");
                PickleModule = GetModule("unity_pickler");
            }
            LoadScriptBundles();
        }

        public static void ConfigureSys() {
            using (Py.GIL()) {
                dynamic sys = Scope.Import("sys");

                sys.path.clear();
                sys.path.append(GetPythonHome());
                sys.path.append(GetPythonHome() + "/Lib");
                sys.path.append(GetPythonHome() + "/Lib/site-packages");
                sys.path.append(GetPythonHome() + "/DLLs");

                sys.path.append(Application.streamingAssetsPath + "/PythonScripts_Internal");
                sys.path.append(Application.streamingAssetsPath + "/PythonScripts_Editor");
                sys.path.append(Application.streamingAssetsPath + "/PythonScripts");
            }
        }

        public static void LoadScriptBundles() {
            using (Py.GIL()) {
#if UNITY_EDITOR
                dynamic sys = Scope.Import("sys");
                sys.path.append(Application.dataPath);
#else
                UnityPy.load_script_bundles(GetPythonScriptBundleDirectory());
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
                return PickleModule.InvokeMethod("dumps", obj).As<byte[]>();
            }
        }

        public static PyObject UnpickleData(byte[] data) {
            using (Py.GIL()) {
                return PickleModule.InvokeMethod("loads", data.ToPython());

            }
        }

        public static void UnpickleDataInto(byte[] data, PyObject obj) {
            using (Py.GIL()) {
                PickleModule.InvokeMethod("loads_into", data.ToPython(), obj);

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
                module = PythonManager.PickleModule.InvokeMethod("get_module", obj).As<string>();
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
