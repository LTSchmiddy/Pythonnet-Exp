using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Python.Runtime;
using PythonEngine;

using AlexEditorUtilities;

namespace PythonEngineEditor {

    public class PythonBuildSettings : CustomProjectSettingsBase<PythonBuildSettings> {
        public override void InitialValues() {}
    }
}