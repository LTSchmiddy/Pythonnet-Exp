using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using PythonEngine;

namespace PythonEngineEditor {
    [CustomEditor(typeof(PythonModuleObject), true)]
    public class PythonModuleObjectInspector : AlexEditorBase<PythonModuleObject> {
        PythonScriptEditor scriptEditor;

        public override void OnEnable() {
            base.OnEnable();
            scriptEditor = new PythonScriptEditor(Target);
        }
        
        public override void OnInspectorGUI_Easy() {
            DrawDefaultInspector();
            GUI.enabled = true;

            scriptEditor.DrawEditor();
            GUI.enabled = false;
        }

    }
}