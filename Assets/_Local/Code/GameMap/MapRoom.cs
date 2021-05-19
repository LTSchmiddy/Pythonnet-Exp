using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameUniverse;
using GameUniverse.SceneTypes;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace GameMap {
    [CreateAssetMenu(menuName="_LOCAL/WorldMap/Map Room", fileName="New Map Room", order=0)]
    public class MapRoom : ScriptableObject {
        public enum GridSelectionType {
            All,
            None,
            Invert
        }

        public const char GRID_DATA_ROW_SEPARATOR = ';';
        public const char GRID_DATA_TRUE = 'o';
        public const char GRID_DATA_FALSE = 'x';

        [SerializeField] protected string displayName;
        [SerializeField] protected MapScene roomScene;
        [SerializeField] protected Vector2Int dimensions;
        // [SerializeField] protected List<List<bool>> grid;
        [SerializeField] protected string gridData;

        public MapScene RoomScene { get => roomScene; set => roomScene = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public Vector2Int Dimensions { get => dimensions; set => dimensions = value; }

        public bool[,] grid {
            get {
                bool[,] retVal = new bool[Dimensions.x,Dimensions.y];

                string[] dataSplit = gridData.Split(GRID_DATA_ROW_SEPARATOR);

                for (int y = 0; y < Dimensions.y && y < dataSplit.Length; y++) {
                    for (int x = 0; x < Dimensions.x && x < dataSplit[y].Length; x++) {
                        retVal[x,y] = dataSplit[y][x] == GRID_DATA_TRUE;
                    }
                }
                return retVal;
            }

            set {
                StringBuilder newData = new StringBuilder((Dimensions.x + 1)* Dimensions.y);
                for (int y = 0; y < Dimensions.y; y++) {
                    for (int x = 0; x < Dimensions.x; x++) {
                        if (x >= value.GetLength(0) || y >= value.GetLength(1)) {
                            newData.Append(GRID_DATA_FALSE);
                        } else {
                            newData.Append(value[x,y] ? GRID_DATA_TRUE : GRID_DATA_FALSE);
                        }
                    }
                    newData.Append(GRID_DATA_ROW_SEPARATOR);
                }
                gridData = newData.ToString();
            }
        }

        public List<List<bool>> gridList {
            get {
                bool[,] currentGrid = grid;
                List<List<bool>> retVal = new List<List<bool>>(currentGrid.GetLength(1));

                for (int y = 0; y < currentGrid.GetLength(1); y++) {
                    List<bool> row = new List<bool>(currentGrid.GetLength(0));
                    for (int x = 0; x < currentGrid.GetLength(0); x++) {
                        row.Add(currentGrid[x,y]);
                    }
                    retVal.Add(row);
                }

                return retVal;
            }

            set {
                bool[,] newGrid = new bool[Dimensions.x, Dimensions.y];

                for (int y = 0; y < newGrid.GetLength(1) && y < value.Count; y++) {
                    for (int x = 0; x < newGrid.GetLength(0) && x < value[y].Count; x++) {
                        newGrid[x, y] = value[y][x];
                    }
                }

                grid = newGrid;
            }
        }

#if UNITY_EDITOR
        public Scene CreateScene(bool keepOpen = false) {
            // Creates new scene:
            Scene retVal = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            
            // Saving the new scene in the same place as this MapRoom object:
            string assetPath = AssetDatabase.GetAssetPath(this);
            string scenePath = assetPath.Substring(0, assetPath.Length - ".asset".Length)  + ".unity";
            EditorSceneManager.SaveScene(retVal, scenePath, false);

            if (!keepOpen) {
                EditorSceneManager.CloseScene(retVal, true);
            }

            // Storing the saved scene:
            RoomScene.ScenePath = scenePath;
            return retVal;
        }
        public void ResizeGrid(Vector2Int newSize) {
            Dimensions = newSize;
            ResizeGrid();
        }

        public void ResizeGrid() {
            // Running both sides of the conversion process should correct the grid's size:
            bool[,] convertedGrid = grid;
            grid = convertedGrid;
        }

        public void ChangeGridSelection(GridSelectionType selectionType) {
            bool[,] newGrid = grid;

            for (int y = 0; y < newGrid.GetLength(1); y++) {
                for (int x = 0; x < newGrid.GetLength(0); x++) {
                    switch (selectionType) {
                        case GridSelectionType.All:
                            newGrid[x, y] = true;
                            break;
                        case GridSelectionType.None:
                            newGrid[x, y] = false;
                            break;
                        case GridSelectionType.Invert:
                            newGrid[x, y] = !newGrid[x, y];
                            break;
                    }
                }
            }

            grid = newGrid;
        }

        public void ShiftGrid(Vector2Int shift) {
            ShiftGrid(shift.x, shift.y);
        }
        public void ShiftGrid(int xShift, int yShift) {
            List<List<bool>> currentGridList = gridList;

            // Shifting on Y is easier. Lets do that first:
            listShifter<List<bool>>(currentGridList, yShift);

            // Now for X:
            foreach (List<bool> row in currentGridList) {
                listShifter(row, xShift);
            }
            gridList = currentGridList;
        }

        private void listShifter<T>(List<T> list, int shift) {
            for (int i = 0; i < Mathf.Abs(shift); i++) {
                // Back to front:
                if (shift > 0) {
                    T end = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    list.Insert(0, end);
                }
                // Front to back:
                else if (shift < 0) {
                    T start = list[0];
                    list.RemoveAt(0);
                    list.Add(start);
                }
            }
        }
#endif
    }
}