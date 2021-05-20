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
    public class MapEditorWindow : EditorWindow {

        // Data Variables:
        private Map selectedMap;
        private SerializedObject serializedMap;
        private AutoPropertyManager props;

        // GUI Layout Variables:
        float roomsListWidth = 350f;
        Vector2 roomsListScroll;
        bool showRoomList;


        public static List<string> NamespacesToGenerate = new List<string>(new string[] {
            "UnityEngine",
            "UnityEditor",
        });

        public Map SelectedMap { 
            get { 
                return selectedMap;
            }
            private set {
                if (selectedMap != value) {
                    if (value == null) {
                        serializedMap = null;
                        props = null;
                    } else {
                        serializedMap = new SerializedObject(value);
                        props = new AutoPropertyManager(serializedMap);
                    }
                }
                selectedMap = value;
            }
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("Alex's Tools/Map Editor")]
        public static void OpenWindow() {
            // Get existing open window or if none, make a new one:
            MapEditorWindow window = (MapEditorWindow)EditorWindow.GetWindow(typeof(MapEditorWindow));
            window.Init();
            window.Show();
        }

        public static void OpenWindow(Map map) {
            MapEditorWindow window = (MapEditorWindow)EditorWindow.GetWindow(typeof(MapEditorWindow));
            window.SelectedMap = map;
            window.Init();
            window.Show();
        }
        
        void Init() {
            titleContent = new GUIContent("Game Map Editor");
        }

        void OnGUI() {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(showRoomList ? "Hide Rooms List" : "Show Rooms List", GUILayout.MaxWidth(180f))) {
                showRoomList = !showRoomList;
            }
            SelectedMap = AlexEditorHelper.AutoObjectLayoutField<Map>("Selected Map:", SelectedMap, false);

            EditorGUILayout.EndHorizontal();
            
            if (SelectedMap == null) {
                return;
            }
            // Hopefully, this should prevent all the NullReferenceExceptions that the window gets when I recompile.
            if (props == null || props.WasObjectLost()){
                props = new AutoPropertyManager(serializedMap);
            }
            if (serializedMap == null) {
                serializedMap = new SerializedObject(SelectedMap);
            }


            
            props.Update();
            // this.position

            if (showRoomList) {
                EditorGUILayout.BeginHorizontal();
                roomsListScroll = EditorGUILayout.BeginScrollView(
                    roomsListScroll,
                    false,
                    true,
                    GUILayout.MaxWidth(roomsListWidth)
                );
                EditorGUILayout.BeginVertical();
                props.LayoutAutoPropertyField("rooms", true, null, GUILayout.MaxWidth(roomsListWidth - 30f));
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.BeginVertical();
            }

            DrawEditorBody();

            // End of Sidebar
            if (showRoomList) {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            props.Apply();
        }

        public void DrawEditorBody() {
            props.LayoutAutoPropertyField("size", true, null, GUILayout.MaxWidth(200f));
        }
    }
}