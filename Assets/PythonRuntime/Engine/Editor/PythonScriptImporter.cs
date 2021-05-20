using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

using Python.Runtime;
using PythonEngine;

namespace PythonEngineEditor {
    [ScriptedImporter(version: 9, ext: "py")]
    public class PythonScriptImporter : ScriptedImporter {
        public override void OnImportAsset(AssetImportContext ctx) {
            // Debug.Log("Importing PyScript: " + ctx.assetPath);
            string code = ctx.assetPath;
            string moduleName = Path.GetFileNameWithoutExtension(ctx.assetPath);

            PythonFile script = PythonFile.New(moduleName, false);

            PyObject analyser = PythonManager.GetModule("unity_python_file_analyser");
            PyObject result = analyser.InvokeMethod("analyse_PythonFile", script.ToPython(), ctx.assetPath.ToPython());

            script.definedFunctions = result.As<string[][]>()[0];
            script.definedClasses = result.As<string[][]>()[1];

            ctx.AddObjectToAsset("script", script);
            ctx.SetMainObject(script);
        }
    }
    [ScriptedImporter(version: 7, ext: "pyc")]
    public class PythonBytecodeImporter : ScriptedImporter {
        public override void OnImportAsset(AssetImportContext ctx) {
            // Debug.Log("Importing PyScript: " + ctx.assetPath);
            string code = ctx.assetPath;
            string moduleName = Path.GetFileNameWithoutExtension(ctx.assetPath);

            PythonFile script = PythonFile.New(moduleName, true);

            ctx.AddObjectToAsset("script", script);
            ctx.SetMainObject(script);
        }

        
    }
}