using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Python.Runtime;

namespace PythonEngine {
    [Serializable]
    public class PythonEvent {
        public string importCode = "from UnityEngine import *";
        public string eventCode;
        public UnityEngine.Object[] objectRefs;

        //
        public PyObject eventModule;
        public PyObject eventFunction;

        public UnityEngine.Object caller {get; set;}

        public PythonEvent() {}
        public PythonEvent(UnityEngine.Object p_Caller = null, string p_ImportCode = null, string p_EventCode = null, UnityEngine.Object[] p_ObjectRefs = null) {
            caller = p_Caller;
            importCode = p_ImportCode;
            eventCode = p_EventCode;
            objectRefs = p_ObjectRefs;
        }

        public void Preload(UnityEngine.Object p_Caller = null) {
            using (Py.GIL()) {
                dynamic factory = Py.Import("inline_module_factory");
                factory.setup_python_event(this);
            }


            caller = p_Caller;
            
        }

        public void Invoke() {
            if (eventFunction == null) {
                Preload();
            }
            using (Py.GIL()) {
                eventFunction.Invoke(objectRefs.ToPython(), caller.ToPython());
            }
        }
    }
}