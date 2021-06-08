using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlexEditorUtilities {
    public abstract class CustomProjectSettingsBase<T> : ScriptableObject where T : CustomProjectSettingsBase<T> {
        public static string GetSettingsName() {return typeof(T).FullName;}
        public abstract void InitialValues();

        public static string GetSettingsPath() {
            return "Assets/Editor/CustomSettings/" + GetSettingsName() + ".asset";
        }

        internal static T GetOrCreateSettings() {
            var settings = AssetDatabase.LoadAssetAtPath<T>(GetSettingsPath());
            if (settings == null) {
                settings = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(settings, GetSettingsPath());
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings() {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}