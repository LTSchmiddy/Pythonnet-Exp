using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PythonEngine;

namespace LoadSave {
    [Serializable]
    public class SaveFile {
        public string profileName;

        public Guid sceneId;

        public Dictionary<Guid, SaveIdRecord> records = new Dictionary<Guid, SaveIdRecord>();

        public SaveIdRecord GetRecords(Guid id) {
            if (!records.ContainsKey(id)) {
                SaveIdRecord retVal = new SaveIdRecord();
                records.Add(id, retVal);
                return retVal;
            } else {
                return records[id];
            }
        }
    }
    
    [Serializable]
    public class SaveIdRecord {
        public Dictionary<string, ISaveableInfo> info = new Dictionary<string, ISaveableInfo>(); 
        public Dictionary<string, SavedPyBehaviourObject> pythonInfo = new Dictionary<string, SavedPyBehaviourObject>();
    }
}