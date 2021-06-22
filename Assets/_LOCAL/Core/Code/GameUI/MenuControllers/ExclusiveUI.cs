using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GameUI {


    // My main goal with the UI system is that I want it to be fully replaceable.
    // None of the runtime code should make any calls to the GUI classes, as much as possible. 
    // If I replace a GUI scene, or decide not to load the GUI for the moment, nothing should complain.

    // The GUI can register its methods to events in the gameplay, but no direct method calls.
    public abstract class ExclusiveUI : MonoBehaviour {

        protected static List<ExclusiveUI> UIs = new List<ExclusiveUI>(4);

        protected static void HideAll() {
            foreach (ExclusiveUI i in UIs) {
                i.IsOpen = false;
            }
        }

        public static ExclusiveUI CurrentUI {
            get {
                foreach (ExclusiveUI i in UIs) {
                    if (i.IsOpen) {
                        return i;
                    }
                }
                return null;
            }
        }

        public bool IsOpen {
            get {
                return gameObject.activeSelf;
            }
            set {
                // No need to call SetShown if nothing's changing;
                if (value != IsOpen) {
                    SetOpen(value);
                }
            }
        }
        
        protected virtual void SetOpen(bool state) {
            if (state) {
                HideAll();
            }

            gameObject.SetActive(state);
        }


        protected virtual void Awake() {
            if (!UIs.Contains(this)) {
                UIs.Add(this);
            }
        }

        protected virtual void OnDestroy() {
            if (UIs.Contains(this)) {
                UIs.Remove(this);   
            }
        }

    }
}