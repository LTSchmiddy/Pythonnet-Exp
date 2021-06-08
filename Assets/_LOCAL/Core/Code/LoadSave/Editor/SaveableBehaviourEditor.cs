using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GameUniverse;
using LoadSave;


namespace LoadSaveEditor {
    
    public class SaveableBehaviourEditorBase<T> : AlexEditorBase<T> where T : SaveableBehaviour {

        public override void OnInspectorGUI() {
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            OnInspectorGUI_Easy();
            if (EditorGUI.EndChangeCheck()) {
                Target.AttemptSaveLink();
                Target.HandleOnEditorChanged();
            }

            serializedObject.ApplyModifiedProperties();
            
        }
    }
    [CustomEditor(typeof(SaveableBehaviour), true)]
    public class SaveableBehaviourEditor : SaveableBehaviourEditorBase<SaveableBehaviour> {}
}