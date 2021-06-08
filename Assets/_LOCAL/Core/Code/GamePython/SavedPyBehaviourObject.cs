using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using RoboRyanTron.QuickButtons;

using LoadSave;
using Python.Runtime;
using PythonEngine;

namespace PythonEngine {
    [Serializable]
    public class SavedPyBehaviourObject : ISaveableInfo {
        public PyObject pyObject;

        public SavedPyBehaviourObject(){}
        public SavedPyBehaviourObject(PyObject pyObj){
            pyObject = pyObj;
        }

        string ISaveableInfo.GetRecordKey() {
            return "SavedPyBehaviourObject";
        }

        public static implicit operator PyObject(SavedPyBehaviourObject d) => d.pyObject;
        public static implicit operator SavedPyBehaviourObject(PyObject d) => new SavedPyBehaviourObject(d);
    }

}