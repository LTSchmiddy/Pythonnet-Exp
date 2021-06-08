using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LoadSave {
    public abstract class SaveableBehaviour : MonoBehaviour, ISaveableBehaviour, ISerializationCallbackReceiver {
        // public abstract string GetRecordKey();
        public abstract void LinkSaveableInfo(SaveIdRecord record);
        protected SaveableId saveId;

        public T PerformLink<T>(Dictionary<string, T> info, T data, string label = null) where T : ISaveableInfo {
            if (string.IsNullOrEmpty(label)) {
                label = data.GetRecordKey();
            }

            if (
                !info.ContainsKey(label)
            ) {
                info.Add(label, data);
                return data;
            } else {
#if UNITY_EDITOR
                // We want to force the default record to reflect what we set up in the Editor.
                if (!EditorApplication.isPlaying) {
                    info[label] = data;
                }
#endif
                return info[label];
            }
        }

        public bool AttemptSaveLink() {
            try {
                if (saveId == null) {
                    saveId = GetComponent<SaveableId>();
                }
            } catch (MissingReferenceException) {
                return false;
            }


            if (saveId == null) {
                return false;
            }
            print("Linking LoadSave Record: " + saveId.id);
            SaveIdRecord record = saveId.GetSaveRecords();
            LinkSaveableInfo(record);
            return true;
        }

        protected virtual void Awake() {
            AttemptSaveLink();
        }

        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            HandleBeforeSerialize();
#endif
        }

        // Called to set up data for deserialization. Stubbed out when not in editor.
        public void OnAfterDeserialize() {
#if UNITY_EDITOR
            // We sadly cannot touch assetdatabase during serialization, so defer by a bit.
            EditorApplication.update += HandleAfterDeserialize;
            // HandleAfterDeserialize();
#endif
        }

#if UNITY_EDITOR
        public void HandleOnEditorChanged() {
            if (
                saveId != null
                && !EditorApplication.isPlaying
            ) {
                Debug.Log("Saving Default Record: " + saveId.id);
                LoadSaveManager.Main.WriteDefaultSaveData(saveId.id);
            }
        }

        private void HandleBeforeSerialize() { }

        private void HandleAfterDeserialize() {
            EditorApplication.update -= HandleAfterDeserialize;
            if (AttemptSaveLink()) {
                HandleOnEditorChanged();
            }

        }
#endif
    }
}