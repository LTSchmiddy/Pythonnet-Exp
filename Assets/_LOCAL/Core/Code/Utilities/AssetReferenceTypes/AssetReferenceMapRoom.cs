using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using GameUniverse;
using GameUniverse.SceneTypes;
using System.Text;

using GameMap;


namespace Utilities.AssetReferenceTypes {
    // I can use this to declare more types of addressable assets to use in the inspector.

    [Serializable]
    public class AssetReferenceMapRoom : AssetReferenceT<MapRoom> {
        public AssetReferenceMapRoom(string guid) : base(guid) {}
    }
}