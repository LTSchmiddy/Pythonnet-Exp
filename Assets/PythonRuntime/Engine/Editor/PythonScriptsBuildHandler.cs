using System.Security.AccessControl;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using UnityEditor.Build.Reporting;

using PythonEngine;
using Python.Runtime;

class PythonScriptsBuildHandler : IPreprocessBuildWithReport {

    public int callbackOrder {
        get {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildReport report) {

        string ExecutableName = Path.GetFileName(report.summary.outputPath);

        BuildTarget target = report.summary.platform;
        string path = report.summary.outputPath;
        
        string buildDir = path.Remove(path.Length - ExecutableName.Length, ExecutableName.Length);

        // PythonEngineEditor.PythonEditorUtilities.DirectoryCopy(PythonManager.GetPythonHome(), buildDir + "/PythonRuntime", true);
    }


    
}