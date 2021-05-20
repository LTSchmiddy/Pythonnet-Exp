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
        Debug.Log("report.summary.outputPath = " + path);
        
        string buildDir = path.Remove(path.Length - ExecutableName.Length, ExecutableName.Length);


        Debug.Log("Making Python Scripts Archive: " + buildDir);

        BuildPythonCodeArchive("./Assets", buildDir + "/core.zip");
        DirectoryCopy("./PythonRuntime", buildDir + "/PythonRuntime", true);
        DirectoryCopy("./PythonScripts", buildDir + "/PythonScripts", true);
        DirectoryCopy("./PythonScripts_Internal", buildDir + "/PythonScripts_Internal", true);
    }


    public static void BuildPythonCodeArchive(string rootDirectory, string outputPath) {
        // It would seem that we're unable to use .Net's ZipFile modules. However, Python's still works.
        // Hence, we're gonna have Python handle the code archive building.

        PyObject builder_module = PythonManager.GetModule("unity_build_code_archive");
        builder_module.InvokeMethod("assemble_python_code_archive", rootDirectory.ToPython(), outputPath.ToPython());
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        DirectoryInfo dest = new DirectoryInfo(destDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        
        // If the destination directory doesn't exist, create it.       
        Directory.CreateDirectory(destDirName);        

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }
}