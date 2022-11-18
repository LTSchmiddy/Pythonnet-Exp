using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

using PythonEngine;

namespace PythonEngineEditor {
    public class PythonScriptEditor {
        protected PythonModuleObject script;

        public PythonScriptEditor(PythonModuleObject p_script) {
            script = p_script;
        }

        public void DrawEditor() {
            if (GUILayout.Button("Save Script")) {
                Save();    
            }
            script.Reference.PyFileInfo.code = EditorGUILayout.TextArea(script.Reference.PyFileInfo.code);
        }


        public void Save() {
            // string[] lines = script.Reference.PyFileInfo.code.Split('\n');
            // File.WriteAllLines("WriteLines.txt", lines);
            File.WriteAllText(script.Reference.ModulePath, script.Reference.PyFileInfo.code);
        }
    }
}