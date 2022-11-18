using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Python.Runtime;

namespace PythonEngine {

    // Originally, I was gonna use this scriptable object to store module paths
    // for Python scripts. However, the qualified module name wasn't being updated if the file was moved.
    // Ergo, we're just using this to ensure that Unity sees Python files as a unique type of asset.
    // For now, it just holds info from the import process. The class "PythonModuleReference" and its subclasses 
    // use this object to ensure they always have correct import path for this python script.
    public class PythonFile : ScriptableObject {
        public string moduleName;

        // Honestly, I'm not sure there's much point to storing the script's actual code,
        // other than maybe displaying it in the editor. Perhaps I should make this property 
        // editor only? ... I'll have to think about it.
        public string code;
        public List<string> definedFunctionNames = new List<string>();
        public List<string> definedClassNames = new List<string>();

        public PythonModuleObject moduleObject;
        
        public static PythonFile New(string p_moduleName) {
            PythonFile retVal = PythonFile.CreateInstance<PythonFile>();
            retVal.Init(p_moduleName);
            return retVal;
        }

        public void Init(string p_moduleName) {
            moduleName = p_moduleName;
        }
    }
}