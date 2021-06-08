using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameSettings {
    [CreateAssetMenu(menuName = "_LOCAL/GameSettings/GameSettings Object", fileName = "GameSettings", order = 0)]
    public class SettingsManager : ScriptableObject {
        public static string SettingsFilePath {
            get => Application.persistentDataPath + "/settings.json";
        }

        public Settings defaultSettings;
        /*[NonSerialized]*/ public Settings settings;

        public void CreateNewSettingsFile() {
            Settings gs = defaultSettings;
            WriteSettingsFile(gs);
        }


        public void WriteSettingsFile() {
            WriteSettingsFile(settings);
        }
        public void WriteSettingsFile(Settings w_settings) {
            if (File.Exists(SettingsFilePath)) {
                File.Delete(SettingsFilePath);
            }

            StreamWriter newFile = File.CreateText(SettingsFilePath);
            newFile.Write(JsonUtility.ToJson(w_settings, true));
            newFile.Close();
        }

        public void GetSettingsFromFile() {
            if (!File.Exists(SettingsFilePath)) {
                CreateNewSettingsFile();
            }

            StreamReader reader = new StreamReader(SettingsFilePath);
            settings = JsonUtility.FromJson<Settings>(reader.ReadToEnd());
            reader.Close();
        }

        public void Apply() {
            settings.Apply();
        }
    }
}
