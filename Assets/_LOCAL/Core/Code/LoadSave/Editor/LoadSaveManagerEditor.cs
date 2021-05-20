using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GameUniverse;
using LoadSave;


namespace LoadSaveEditor {
    [CustomEditor(typeof(LoadSaveManager), true)]
    public class LoadSaveManagerEditor : AlexEditorBase<LoadSaveManager> {

        public enum PostRecordAction {
            NONE,
            DELETE,
        }

        public override void OnInspectorGUI_Easy() {
            base.OnInspectorGUI_Easy();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save Data:");
            
            foreach(KeyValuePair<Guid, SaveIdRecord> entry in Target.data.records) {
                DrawRecordInspector(entry.Key, entry.Value);
                
            }

        }
        

        // Adding functions to draw data for subtypes; Static, so that they could be potentially used by other editor scripts.
        public static void DrawRecordInspector(Guid guid, SaveIdRecord record) {

        }
        
    }
}

