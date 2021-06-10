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
    [ScriptedImporter(version: 19, ext: "py")]
    public class PythonScriptImporter : ScriptedImporter {
        public override void OnImportAsset(AssetImportContext ctx) {
            if(!ctx.assetPath.StartsWith("Assets/")){
                Debug.Log("PyScript '" + ctx.assetPath + "' Not in Assets Folder...");
                return;
            }

            string moduleName = Path.GetFileNameWithoutExtension(ctx.assetPath);

            // Parsing Python Code:
            PythonFile script = PythonFile.New(moduleName);
            PyObject result;
            using (Py.GIL()) {
                result = PyUtil.Invoke("unity_python_file_analyser", "analyse_PythonFile", script.ToPython(), ctx.assetPath.ToPython());
            }
            script.definedFunctionNames = new List<string>(result.As<string[][]>()[0]);
            script.definedClassNames = new List<string>(result.As<string[][]>()[1]);

            // Creating Module Object
            PythonModuleObject module = PythonModuleObject.EditorNew(script);
            ctx.AddObjectToAsset("module", module);
            ctx.SetMainObject(module);

            script.name = "(Script Info) " + moduleName;
            ctx.AddObjectToAsset("script", script);

            module.classes = new List<PythonClassObject>(script.definedClassNames.Count);
            // Creating Class Objects:
            foreach (string i in script.definedClassNames){
                PythonClassObject classObj = PythonClassObject.EditorNew(script, i);
                classObj.name = "(Class) " + i;
                ctx.AddObjectToAsset(classObj.name, classObj);
                module.classes.Add(classObj);
            }

            // Creating Function Objects:
            module.functions = new List<PythonFunctionObject>(script.definedFunctionNames.Count);
            foreach (string i in script.definedFunctionNames){
                PythonFunctionObject fnObj = PythonFunctionObject.EditorNew(script, i);
                fnObj.name = "(Function) " + i;
                ctx.AddObjectToAsset(fnObj.name, fnObj);
                module.functions.Add(fnObj);
            }
            

            
            
            

            
        }
    }
}