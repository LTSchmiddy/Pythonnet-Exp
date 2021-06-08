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

                UnityEngine.Debug.Log("Rebuilding Python Stubs");
            } catch (Exception e) {
                UnityEngine.Debug.LogError("Unable to launch stub generator: " + e.Message);
            }
        }

        static void DataReceived(object sender, DataReceivedEventArgs eventArgs) {
            // UnityEngine.Debug.Log( eventArgs.Data);// Handle it
        }

        static void ErrorReceived(object sender, DataReceivedEventArgs eventArgs) {
            UnityEngine.Debug.LogError(eventArgs.Data);
        }

        public static void BuildPythonCodeArchive(string rootDirectory, string outputPath) {
            // It would seem that we're unable to use .Net's ZipFile modules. However, Python's still works.
            // Hence, we're gonna have Python handle the code archive building.

            PyObject builder_module = PythonManager.GetModule("unity_build_code_archive");
            builder_module.InvokeMethod("assemble_python_code_archive", rootDirectory.ToPython(), outputPath.ToPython());
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            DirectoryInfo dest = new DirectoryInfo(destDirName);

            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}