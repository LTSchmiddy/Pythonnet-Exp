using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;
using GameUniverse.SceneTypes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameMap {
    [CreateAssetMenu(menuName="_LOCAL/WorldMap/MapData Object", fileName="MapDataObject", order=2)]
    public class MapData : ScriptableObject
    {
        public const string GLOBAL_MAPDATA_PREFAB_PATH = "_LOCAL/MapDataObject";
        // Start is called before the first frame update
        public static MapData Prefab {
            get => Resources.Load<MapData>(GLOBAL_MAPDATA_PREFAB_PATH);
        }
        public static MapData Main {
            get {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    return Prefab;
                }
#endif
                if (GlobalManager.Instance == null) {
                    return null;
                }
                return GlobalManager.Instance.mapData;
            }
        }
        
        public MapScene[] scenes = new MapScene[0];
        // public MapScene map;

    }
}
