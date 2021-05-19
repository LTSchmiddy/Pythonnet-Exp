using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameUniverse;
using GameUniverse.SceneTypes;
using GameMap;

using UnityEditor;
using UnityEditor.SceneManagement;


namespace GameMapEditor {
    [CustomEditor(typeof(MapRoom), true)]
    public class MapRoomEditor : AlexEditorBase<MapRoom> {
        const float GRID_TOGGLE_LEFT_PADDING = 20f;
        const float GRID_TOGGLE_TOP_PADDING = 10f;
        const float GRID_TOGGLE_RIGHT_PADDING = 20f;
        const float GRID_TOGGLE_BOTTOM_PADDING = 10f;

        const float GRID_TOGGLE_X_SIZE = 20f;
        const float GRID_TOGGLE_Y_SIZE = 20f;
        bool[,] grid;

        Vector2 gridViewScroll;

        public override void OnInspectorGUI_Easy() {


            AutoPropertyField("displayName");
            AutoPropertyField("roomScene");
            if (String.IsNullOrEmpty(Target.RoomScene.ScenePath)) {
                if (GUILayout.Button("Create Unity Scene")) {
                    Target.CreateScene();
                }
            } else if (GUILayout.Button("Open Scene")) {
                EditorSceneManager.OpenScene(Target.RoomScene.ScenePath, OpenSceneMode.Additive);
            }

            EditorGUILayout.Space();
            AutoPropertyField("dimensions");
            if (grid == null) {
                grid = Target.grid;
            }
            
            EditorGUILayout.Space();
            
            // Grid Editor Controls:
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Resize Grid")) {
                Target.ResizeGrid();
                RefreshGridView();
            }
            if (GUILayout.Button("Shift Left")) {
                Target.ShiftGrid(-1, 0);
                RefreshGridView();
            }
            if (GUILayout.Button("Shift Up")) {
                Target.ShiftGrid(0, 1);
                RefreshGridView();
            }
            if (GUILayout.Button("Shift Down")) {
                Target.ShiftGrid(0, -1);
                RefreshGridView();
            }
            if (GUILayout.Button("Shift Right")) {
                Target.ShiftGrid(1, 0);
                RefreshGridView();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All")) {
                Target.ChangeGridSelection(MapRoom.GridSelectionType.All);
                RefreshGridView();
            }
            if (GUILayout.Button("Select None")) {
                Target.ChangeGridSelection(MapRoom.GridSelectionType.None);
                RefreshGridView();
            }           
            if (GUILayout.Button("Invert Selection")) {
                Target.ChangeGridSelection(MapRoom.GridSelectionType.Invert);
                RefreshGridView();
            }
            EditorGUILayout.EndHorizontal();

            // DrawDefaultInspector();
            EditorGUILayout.Separator();

            // Draw map grid
            AutoPropertyField("gridData");
            EditorGUI.BeginChangeCheck();
            DrawGridView();
            if (EditorGUI.EndChangeCheck())
            {
                Target.grid = grid;
            }
        }

        public void DrawGridView() {
            gridViewScroll = EditorGUILayout.BeginScrollView(
                gridViewScroll,
                true,
                true,
                // GUILayout.MaxWidth(((grid.GetLength(0) + 1)* GRID_TOGGLE_X_SIZE) + GRID_TOGGLE_LEFT_PADDING + GRID_TOGGLE_RIGHT_PADDING),
                GUILayout.MaxHeight(((grid.GetLength(1) + 1)* GRID_TOGGLE_Y_SIZE) + GRID_TOGGLE_TOP_PADDING + GRID_TOGGLE_BOTTOM_PADDING)
            );
            // DrawGridLayout();
            DrawGrid();

            EditorGUILayout.EndScrollView();
        }

        void DrawGrid(){
            for (int y = grid.GetLength(1) - 1; y >= 0; y--) {
                for (int x = 0; x < grid.GetLength(0); x++) {
                    Rect pos = new Rect(
                        (x * GRID_TOGGLE_X_SIZE) + GRID_TOGGLE_LEFT_PADDING,
                        (y * GRID_TOGGLE_Y_SIZE) + GRID_TOGGLE_TOP_PADDING,
                        GRID_TOGGLE_X_SIZE,
                        GRID_TOGGLE_Y_SIZE
                    );

                    grid[x,y] = EditorGUI.Toggle(pos, grid[x,y]);
                }
            }
        }

        void DrawGridLayout(){
            for (int y = grid.GetLength(1) - 1; y >= 0; y--) {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < grid.GetLength(0); x++) {
                    grid[x,y] = EditorGUILayout.Toggle(grid[x,y]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        public void RefreshGridView() {
            grid = Target.grid;
        }
    }
}