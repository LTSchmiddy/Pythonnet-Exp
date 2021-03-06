using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Python.Runtime;
using PythonEngine;

namespace PythonEngineEditor {
     [InitializeOnLoad]
    public class PythonToolsWindow : EditorWindow {
        bool debugStubGen = false;

        public static List<string> NamespacesToGenerate = new List<string>(new string[] {
            "UnityEngine",
            "UnityEditor",
        });

        // Add menu named "My Window" to the Window menu
        [MenuItem("Python/Python Tools Window")]
        static void OpenWindow() {
            // Get existing open window or if none, make a new one:
            PythonToolsWindow window = (PythonToolsWindow)EditorWindow.GetWindow(typeof(PythonToolsWindow));
            window.Init();
            window.Show();
        }

        void Init() {
            titleContent = new GUIContent("Python Tools Window");
            if (PythonManager.IsInitialized) {
                InitPythonData();
            }
        }

        void InitPythonData() {
            
        }

        void OnGUI() {
            EditorGUILayout.LabelField("Is Initialized: " + PythonManager.IsInitialized);
            EditorGUILayout.BeginHorizontal();
            {
                if (PythonManager.IsInitialized){
                    if (GUILayout.Button("Shutdown Python")) {
                        PythonManager.Uninitialize();
                    }
                    if (GUILayout.Button("Reload Python")) {
                        PythonManager.Uninitialize();
                        PythonManager.Initialize();
                        InitPythonData();
                    }
                } else if (GUILayout.Button("Initialize Python")) {
                    PythonManager.Initialize();
                    InitPythonData();
                }
            } 
            EditorGUILayout.EndHorizontal();
            
            debugStubGen = EditorGUILayout.Toggle("Debug Stub Gen", debugStubGen);
            if (GUILayout.Button("Run Stub Generation")) {
                PythonEditorUtilities.UpdateStubs();
            }
        }
    }
}