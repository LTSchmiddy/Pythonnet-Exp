using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;
using GameUniverse.SceneTypes;
using GameMap;

using UnityEditor;


namespace GameMapEditor {
    [CustomEditor(typeof(MapRoom), true)]
    public class MapRoomEditor : AlexEditorBase<MapRoom> {

        public override void OnInspectorGUI_Easy() {
            AutoPropertyField("displayName");
            AutoPropertyField("scene");
            if (String.IsNullOrEmpty(Target.Scene.ScenePath) && GUILayout.Button("Create Unity Scene")) {
                // Fill this in later
            }
            AutoPropertyField("dimensions");
            EditorGUILayout.Separator();

            

        }
    }
}