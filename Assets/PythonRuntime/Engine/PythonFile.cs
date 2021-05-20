using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Python.Runtime;

namespace PythonEngine {

    // Originally, I was gonna use this scriptable object to store module paths
    // for Python scripts. However, the qualified module name wasn't being updated if the file was moved.
    // Ergo, we're just using this to ensure that Unity sees Python files as a unique type of asset.
    // Perhaps, later, these two classes can have their functionality merged.
    // For now, I could use it to
    public class PythonFile : ScriptableObject {
        public string moduleName;
        public bool isBytecode = false;

        // public List<string> definedFunctions = new List<string>();
        // public List<string> definedClasses = new List<string>();

        public string[] definedFunctions;
        public string[] definedClasses;
        
        public static PythonFile New(string p_moduleName, bool p_isBytecode = false) {
            PythonFile retVal = PythonFile.CreateInstance<PythonFile>();
            retVal.Init(p_moduleName, p_isBytecode);
            return retVal;
        }

        public void Init(string p_moduleName, bool p_isBytecode = false) {
            moduleName = p_moduleName;
            isBytecode = p_isBytecode;
            
        }
    }
}