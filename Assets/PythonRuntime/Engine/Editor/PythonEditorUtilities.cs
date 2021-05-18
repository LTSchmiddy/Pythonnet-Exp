using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Python.Runtime;
using PythonEngine;



namespace PythonEngineEditor {
    [InitializeOnLoad]
    public class PythonEditorUtilities {

        public static bool showStubGenConsole = true;
        public static bool rebuildStubsOnReload = false;

        static Process process = null;

        static PythonEditorUtilities() {
            if (!EditorApplication.isPlaying) {
                PythonManager.Reinitialize();
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptsReloaded() {
            if (rebuildStubsOnReload) {
                UpdateStubs();
            }
        }

#if UNITY_EDITOR
        [MenuItem("Python/Update Python Stubs")]
#endif
        public static void UpdateStubs() {
            
            // StreamWriter messageStream;
            try {
                process = new Process();
                process.EnableRaisingEvents = false;
                process.StartInfo.FileName = Directory.GetCurrentDirectory() + "/Tools/ironstub-generator/" + (showStubGenConsole ? "ipy.exe" : "ipyw.exe");
                process.StartInfo.Arguments = "build_stubs_for_all_asm.py --parallel --update";
                process.StartInfo.WorkingDirectory = "./Tools/ironstub-generator/";
                process.StartInfo.UseShellExecute = true;
                process.Start();
        
                UnityEngine.Debug.Log( "Rebuilding Python Stubs" );
            }
            catch( Exception e )
            {
                UnityEngine.Debug.LogError( "Unable to launch stub generator: " + e.Message );
            }
        }


        static void DataReceived( object sender, DataReceivedEventArgs eventArgs ) {
            // UnityEngine.Debug.Log( eventArgs.Data);// Handle it
        }
    
    
        static void ErrorReceived( object sender, DataReceivedEventArgs eventArgs ) {
            UnityEngine.Debug.LogError( eventArgs.Data );
        }
    }
}