using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

using GameUniverse;
using GameUI;

namespace GameUIEditor {
    [CustomEditor(typeof(OptionsMenu), true)]
    // public class PythonUIButtonEditor : AlexEditorBase<PythonUIButton> {
    public class OptionsMenuEditor : AlexEditorBase<OptionsMenu> {


        public override void OnInspectorGUI_Easy() {
            // EditorGUI
            Target.CurrentPanelId = (OptionsMenu.SettingsPanel)EditorGUILayout.EnumPopup("Current Settings Panel", Target.CurrentPanelId);
            DrawDefaultInspector();
        }
    }
}

