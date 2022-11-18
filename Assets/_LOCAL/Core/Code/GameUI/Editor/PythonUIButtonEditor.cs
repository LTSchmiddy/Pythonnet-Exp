using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

using GameUniverse;
using GameUI;

namespace GameUIEditor {
    [CustomEditor(typeof(PythonButton), true)]
    // public class PythonUIButtonEditor : AlexEditorBase<PythonUIButton> {
    public class PythonUIButtonEditor : SelectableEditor {

        SerializedProperty m_OnClickPyProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickPyProperty = serializedObject.FindProperty("m_OnClickPy");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_OnClickPyProperty);
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}

