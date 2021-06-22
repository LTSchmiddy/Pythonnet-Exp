using System.Net.Mime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUniverse;
using GameMap;
using BayatGames.SaveGamePro;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using BayatGames.SaveGamePro.Editor;
using UnityEditor;
#endif

namespace LoadSave {
    [CreateAssetMenu(menuName="_LOCAL/LoadSave/LoadSaveData Object", fileName="LoadSaveDataObject", order=0)]
    public class LoadSaveManager : ScriptableObject
    {
        public const string DEFAULT_SCENE_RECORDS_PATH = "DefaultSceneRecords/";
        public const string DEFAULT_RECORDS_PATH = "DefaultRecords/";

        public const string GLOBAL_MANAGER_PREFAB_PATH = "_LOCAL/LoadSaveDataObject";
        // Start is called before the first frame update
        public static LoadSaveManager Prefab {
            get => Resources.Load<LoadSaveManager>(GLOBAL_MANAGER_PREFAB_PATH);
        }
        public static LoadSaveManager Main {
            get {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    return Prefab;
                }
#endif
                if (GlobalManager.Instance == null) {
                    return null;
                }
                return GlobalManager.LoadSave;
            }
        }

        public enum LoadDefaultRecordsMode {
            ClearAllExisting,
            SkipExisting,
            OverwriteExisting
        }

        
        // Editor Properties
        public AR_MapRoom gameStartRoom;

        public SaveFile data = new SaveFile();

        public string GetGameDirectory() {
            // #if UNITY_EDITOR
            //     return System.IO.Directory.GetCurrentDirectory();
            // #else
            //     // return Application.StartupPath;
            //     return Application.dataPath;
            // #endif

            return Application.streamingAssetsPath;
        }

        public SaveGameSettings GetDefaultRecordSettings() {
            SaveGameSettings retVal = SaveGame.DefaultSettings;
            retVal.BasePath = GetGameDirectory();
            // retVal.Formatter = new BayatGames.SaveGamePro.Serialization.Formatters.Json.JsonFormatter ();

            return retVal;
        }

        public void ConstructNewSave() {
            data = new SaveFile();
            
            data.sceneId = gameStartRoom.AssetSync.RoomId;
            
            LoadDefaultRecords(LoadDefaultRecordsMode.ClearAllExisting);
        }

        public void LoadDefaultRecords(LoadDefaultRecordsMode mode = LoadDefaultRecordsMode.ClearAllExisting) {
            if (mode == LoadDefaultRecordsMode.ClearAllExisting) {
                // Clears the dictionary, if required:
                data.records.Clear();
            }

            SaveGameSettings default_record_settings = GetDefaultRecordSettings();
            foreach (FileInfo i in SaveGame.GetFiles(DEFAULT_RECORDS_PATH, default_record_settings)) {
                if (i.Extension == "meta"){
                    continue;
                }

                Debug.Log("Loading ID: " + i.Name);
                
                // Load Guid and Record:
                Guid id = Guid.Parse(i.Name);
                SaveIdRecord record = SaveGame.Load<SaveIdRecord>(DEFAULT_RECORDS_PATH + i.Name, null, default_record_settings);
                
                // If the key doesn't exist, add it:
                if (!data.records.ContainsKey(id)){
                    data.records.Add(id, record);
                }
                // If we're overwriting existing keys:
                else if (data.records.ContainsKey(id) && mode == LoadDefaultRecordsMode.OverwriteExisting) {
                    data.records[id] = record;
                }
            }
        }

#if UNITY_EDITOR
        public void WriteAllDefaultSaveData() {
            foreach(KeyValuePair<Guid, SaveIdRecord> entry in data.records) {
                WriteDefaultSaveData(entry.Key, entry.Value);
                
            }
        }

        public void WriteDefaultSaveData(Guid id) {
            WriteDefaultSaveData(id, data.records[id]);
        }

        public void WriteDefaultSaveData(Guid id, SaveIdRecord record) {
            SaveGameSettings default_record_settings = GetDefaultRecordSettings();

            SaveGame.Save(DEFAULT_RECORDS_PATH + id.ToString(), record, default_record_settings);
        }
#endif
    }
}
