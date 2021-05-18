using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;
using GameUniverse.SceneTypes;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameMap {
    [CreateAssetMenu(menuName="_LOCAL/WorldMap/Map Room", fileName="New Map Room", order=1)]
    public class MapRoom : ScriptableObject {
        [SerializeField] private string displayName;
        [SerializeField] private MapScene scene;
        [SerializeField] private Vector2 dimensions;

        public MapScene Scene { get => scene; set => scene = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public Vector2 Dimensions { get => dimensions; set => dimensions = value; }
    }
}