using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoadSave {
    [Serializable]
    public class SaveFile {
        public string profileName;

        public Dictionary<Guid, SaveIdRecord> records;
    }

    public class SaveIdRecord {
        public Dictionary<string, ISaveableInfo> info;
    }
}