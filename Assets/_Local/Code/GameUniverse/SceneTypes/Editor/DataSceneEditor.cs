using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;

using GameUniverse.SceneTypes;

namespace GameUniverseEditor {


    [CustomPropertyDrawer(typeof(DataScene))]
    public class DataSceneEditor : SceneTypeBasePropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            var loadModeProperty = property.FindPropertyRelative("loadAsync");

            EditorGUILayout.PropertyField(loadModeProperty);

        }
    }
}

