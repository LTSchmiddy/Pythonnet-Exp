using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;

using GameUniverse.SceneTypes;

namespace GameUniverseEditor {


    [CustomPropertyDrawer(typeof(SceneTypeBase), true)]
    public class SceneTypeBasePropertyDrawer : SceneReferencePropertyDrawer {
        
    }
}

