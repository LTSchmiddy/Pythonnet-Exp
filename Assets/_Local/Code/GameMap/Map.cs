using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMap {
    [CreateAssetMenu(menuName="_LOCAL/WorldMap/Map", fileName="New Map", order=1)]
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

        
        public Vector2Int size;
        public RoomEntry[] rooms;

    }
}
