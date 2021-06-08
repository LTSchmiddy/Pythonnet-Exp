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
        //Debug.Log(report.summary);
        //}

        string ExecutableName = Path.GetFileName(report.summary.outputPath);

        BuildTarget target = report.summary.platform;
        string path = report.summary.outputPath;
        // Debug.Log("report.summary.outputPath = " + path);
        
        string buildDir = path.Remove(path.Length - ExecutableName.Length, ExecutableName.Length);


        Debug.Log("Making Python Scripts Archive: " + buildDir);

        // BuildPythonCodeArchive("./Assets", buildDir + "/core.zip");
        // DirectoryCopy("./PythonRuntime", buildDir + "/PythonRuntime", true);
        // DirectoryCopy("./PythonScripts", buildDir + "/PythonScripts", true);
        // DirectoryCopy("./PythonScripts_Internal", buildDir + "/PythonScripts_Internal", true);
    }


    
}