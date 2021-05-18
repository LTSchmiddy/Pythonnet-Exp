using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using GameUniverseEditor;
using GameMap;
using GameUniverse.SceneTypes;

namespace GameMapEditor {


    [CustomPropertyDrawer(typeof(MapScene), true)]
    public class MapScenePropertyDrawer : SceneTypeBasePropertyDrawer {

        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

        }
    }
}

