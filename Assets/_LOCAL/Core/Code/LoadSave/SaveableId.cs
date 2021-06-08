using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using RoboRyanTron.QuickButtons;

using Utilities;
using Python.Runtime;
using PythonEngine;

namespace LoadSave {
    public class SaveableId : MonoBehaviour {
        public SerializableGuid id;

        public SaveIdRecord GetSaveRecords() {
            return LoadSaveManager.Main.data.GetRecords(id);
        }
    }
}