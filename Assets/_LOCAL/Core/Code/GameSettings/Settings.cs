using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using GameUniverse;

namespace GameSettings {
    
    public interface IGameSettingType {
        string GetName();
        void Apply();
    }

    [Serializable]
    public struct Settings : IGameSettingType {

        public DisplaySettings display;
        public AudioSettings audio;

        public InputActionMap inputActions;

        public void Apply() {
            display.Apply();
            audio.Apply();
            
        }


        public string GetName() {
            return "Settings";
        }
    }
}