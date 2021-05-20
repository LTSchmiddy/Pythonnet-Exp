using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadSave {
    public interface ISaveableBehaviour {
        
        // Uses the referential nature of 
        void LinkSaveableInfo(Dictionary<string, ISaveableInfo> info);

    }
}