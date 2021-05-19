using System.Reflection.Emit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;
using GameUniverse.SceneTypes;
using GameMap;

using UnityEditor;


namespace GameMapEditor {
    [CustomEditor(typeof(GameMap.Map), true)]
    public class MapEditor : AlexEditorBase<GameMap.Map> {

        public override void OnInspectorGUI_Easy() {
            if (GUILayout.Button("Open Map Editor Window")) {
                MapEditorWindow.OpenWindow(Target);
            }
            DrawDefaultInspector();
        }
    }
}