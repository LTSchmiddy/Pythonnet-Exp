using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace GameMap {

    [Serializable]
    public class AR_Map : AssetReferenceT<Map> {
        public AR_Map(string guid) : base(guid) { }
    }

    [CreateAssetMenu(menuName = "_LOCAL/WorldMap/Map", fileName = "New Map", order = 1)]
    public class Map : ScriptableObject {
        [Serializable]
        public class RoomEntry {
            public string name {
                get {
                    if (room == null) {
                        return "-- None --";
                    }

                    return room.DisplayName;
                }
            }
            public MapRoom room;
            public Vector2Int position;
        }

        public class GridSlot {
            public string name {
                get {
                    if (room == null) {
                        return "-- None --";
                    }

                    return room.DisplayName;
                }
            }
            public MapRoom room;
            public Vector2Int roomPosition;

            public GridSlot() { }
            public GridSlot(MapRoom p_room, Vector2Int p_roomPosition) {
                room = p_room;
                roomPosition = p_roomPosition;
            }

        }

        // Fields:
        public string displayName;
        public Vector2Int dimensions;
        public RoomEntry[] rooms;



        // Properties:
        public GridSlot[,] Grid { get; private set; }

        /// <summary>
        /// Takes the raw gridData and assembles all variants of that data needed at runtime.
        /// Specifically, We need to unpack the individual mapRooms and add them on the map grid.
        /// </summary>
        public void Unpack() {
            // Instanciating the grid:
            Grid = new GridSlot[dimensions.x, dimensions.y];


            foreach (RoomEntry i in rooms) {
                // Unpacking the room first:
                i.room.Unpack();
                i.room.RegisterAsMapMember(this);

                // For each active grid square in each room, we need to construct the
                // corresponding grid slot
                for (int y = 0; y < i.room.Grid.GetLength(1); y++) {
                    for (int x = 0; x < i.room.Grid.GetLength(0); x++) {
                        // Skipping inactive grid positions:
                        if (!i.room.Grid[x, y]) { continue; }

                        Vector2Int mapPos = i.position + new Vector2Int(x, y);

                        try {
                            // If there's an overlap, report it as an error and don't override the old room:
                            if (Grid[mapPos.x, mapPos.y] != null) {
                                Debug.LogError("Overlapping rooms on map '" + displayName + "' (" + name + "): \n"
                                    + "Position: " + x + ", " + y + "\n"
                                    + "Old Room: " + Grid[mapPos.x, mapPos.y].name + "\n"
                                    + "New Room: " + i.name + "\n"
                                );
                                continue;
                            }
                            
                        } catch(IndexOutOfRangeException) {
                            // Also deal with the room being off the map:
                            
                            Debug.LogError("Out of bounds room position on map '" + displayName + "' (" + name + "): \n"
                                + "Map Position: " + mapPos.x + ", " + mapPos.y + "\n"
                                + "Map Dimensions: " + dimensions.x + ", " + dimensions.y + "\n"
                            );
                            continue;
                        }

                        Grid[mapPos.x, mapPos.y] = new GridSlot(i.room, new Vector2Int(x, y));
                    }
                }
            }
        }
    }
}
