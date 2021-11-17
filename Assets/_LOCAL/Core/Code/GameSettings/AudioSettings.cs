using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameSettings {
    [System.Serializable]
    public struct Volume {
        [Range(0f, 1f)] public float value;

        public float Value {
            get {return Mathf.Clamp01(value);}
            set { this.value = Mathf.Clamp01(value);}
        }

        public Volume(float startVal = 1f) {
            value = Mathf.Clamp01(startVal);
        }

        public static implicit operator float(Volume v) {
            return v.Value;
        }
        public static implicit operator Volume(float v) {
            return new Volume(v);
        }
    }


    [System.Serializable]
    public struct AudioSettings : IGameSettingType {
        public Volume masterVolume;
        public Volume musicVolume;
        public Volume guiVolume;
        public Volume sfxVolume;
        public Volume environVolume;

        public void Apply() {}

        public string GetName() {
            return "Audio";
        }
    }
}
