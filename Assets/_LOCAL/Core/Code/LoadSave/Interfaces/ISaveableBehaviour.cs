using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadSave {
    public interface ISaveableBehaviour {
        
        void LinkSaveableInfo(SaveIdRecord record);

        // string GetRecordKey();

    }
}