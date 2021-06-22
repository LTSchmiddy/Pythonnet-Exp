using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using GameUniverse;
using GameUniverse.SceneTypes;
using System;

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
                return GlobalManager.Map;
            }
        }
        
        // Serialized Fields:
        public List<AR_Map> arMaps = new List<AR_Map>(0);
        
        // Private Fields
        // All known maps:
        private List<Map> maps = new List<Map>();
        // All known rooms, regardless of map:
        private Dictionary<Guid, MapRoom> rooms = new Dictionary<Guid, MapRoom>();

        private List<AsyncOperationHandle<Map>> loadingOperations = new List<AsyncOperationHandle<Map>>(0);
        // Properties:

        public List<Map> Maps { get => maps; private set => maps = value; }

        /// <summary>
        /// What percentage of the last set of load operations have been completed.
        /// Note that this doesn't track whether or not maps have been unpacked.
        /// Use IsLoaded to check whether all map data is ready.
        /// </summary>
        public float percentLoaded {
            get {
                float total = 0f;
                foreach (AsyncOperationHandle<Map> i in loadingOperations) {
                    total += i.PercentComplete;
                }

                return total / (float)loadingOperations.Count;
            }
        }


        private bool isLoaded = false;

        public MapRoom CurrentRoom { get; private set; }
        public bool IsLoaded { get => isLoaded; private set => isLoaded = value; }

        #region Initialization
        /// <summary>
        /// Have all maps been loaded and unpacked?
        /// </summary>

        // Methods:
        public void Init() {
            CurrentRoom = null;
            LoadMapData();
        }

        public void LoadMapData() {
            GlobalManager.GlobalCoroutine(LoadMapDataRoutine());
        }

        IEnumerator LoadMapDataRoutine() {
            IsLoaded = false;
            if (Maps.Count == 0){
                // Allocating size for the map list:
                Maps = new List<Map>(arMaps.Count); 
            }

            // A list for tracking the loading operations:
            loadingOperations = new List<AsyncOperationHandle<Map>>(arMaps.Count);
            
            // Starting the load process for each map:
            foreach (AR_Map i in arMaps) {
                AsyncOperationHandle<Map> loadMapOp = i.LoadAssetAsync();
                // I could use this callback, but I want to make sure that the maps are 
                // unpacked before I start the linking process.
                // loadMapOp.Completed += (AsyncOperationHandle<Map> op) => {}; 
                loadingOperations.Add(loadMapOp);
            }

            // Waiting for all maps to load;
            foreach (AsyncOperationHandle<Map> op in loadingOperations) {
                while(!op.IsDone) { yield return null; }
                // Let's unpack the map before adding it to the list.
                op.Result.Unpack();
                foreach(var i in op.Result.rooms) {
                    rooms.Add(i.room.RoomId, i.room);
                }
                maps.Add(op.Result);
                Debug.Log("Map Loaded and Unpacked: " + op.Result.name);
            }

            // This is where the linking process will be executed... later.
            Debug.Log("Linking Maps...");
            IsLoaded = true;
        }
        #endregion

        public void GoToRoom(MapRoom room) {
            GlobalManager.LoadingScreenOperation(GoToRoomRoutine(room));
        }

        public void GoToRoom(string roomId) {
            MapRoom room = rooms[Guid.Parse(roomId)];
            GlobalManager.LoadingScreenOperation(GoToRoomRoutine(room));
        }
        public void GoToRoom(Guid roomId) {
            MapRoom room = rooms[roomId];
            var c = GlobalManager.LoadingScreenOperation(GoToRoomRoutine(room));
            
        }

        IEnumerator GoToRoomRoutine(MapRoom room) {
            while(!IsLoaded) { yield return null; }

            Debug.Log("Room Load Process: Start");
            if (CurrentRoom != null) {
                Debug.Log("Unloading...");
                var unloadTask = CurrentRoom.UnloadRoom();

                while(!unloadTask.IsDone) {
                    yield return null;
                }
            }

            CurrentRoom = room;
            Debug.Log("Loading...");
            var loadTask = CurrentRoom.LoadRoom();
            while(!loadTask.IsDone) {
                yield return null;
            }

            GlobalManager.LoadSave.data.sceneId = room.RoomId;
            Debug.Log("Room Load Process: End");         
        }
    }
}
