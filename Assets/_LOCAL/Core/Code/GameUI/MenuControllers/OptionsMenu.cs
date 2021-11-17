using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GameUniverse;
using GameSettings;
using Utilities;

namespace GameUI {

    public class OptionsMenu : ExclusiveUI {
        #region Static Handling
        private static OptionsMenu _instance;
        public static OptionsMenu Instance { get => _instance; private set => _instance = value; }
        public static void Show() {
            Instance.IsOpen = true;
        }
        public static void Hide() {
            Instance.IsOpen = false;
        }
        #endregion

        [Header("Category Selection")]
        public Selectable backButton;
        public Selectable displayCategoryButton;
        public Selectable audioCategoryButton;

        [Header("Display Settings")]
        public GameObject displayPanel;
        public TMP_Dropdown displaySelection;
        public TMP_Dropdown fullscreenSelection;

        [Header("Audio Panel")]
        public GameObject audioPanel;

        [Header("New Settings")]
        public Settings newSettings;

        // Runtime Values
        private ExclusiveUI lastUI { get; set; }
        private bool requested = false;
        private GameObject[] panels;

        public OptionsMenu() {
            Instance = this;
        }

        #region Resolution Selection
        protected void ConstructResolutionMenu() {
            displaySelection.options.Clear();
            // foreach (Resolution res in Screen.resolutions) {
            for (int i = 0; i < Screen.resolutions.Length; i++) {
                Resolution res = Screen.resolutions[i];

                TMP_Dropdown.OptionData entry = new TMP_Dropdown.OptionData(res.ToString());
                displaySelection.options.Add(entry);

                if (
                    res.height == newSettings.display.height
                    && res.width == newSettings.display.width
                    && res.refreshRate == newSettings.display.refreshRate
                ) {
                    displaySelection.value = i;
                }
            }
            displaySelection.RefreshShownValue();
        }
        public void OnSelectResolution(int selection) {
            Resolution res = Screen.resolutions[selection];

            newSettings.display.height = res.height;
            newSettings.display.width = res.width;
            newSettings.display.refreshRate = res.refreshRate;
        }
        #endregion

        #region Fullscreen Selection
        protected void ConstructFullscreenMenu() {
            fullscreenSelection.options.Clear();
            foreach (FullScreenMode fs in Enum.GetValues(typeof(FullScreenMode))) {
                string j = UtilityFunctions.AddSpacesToSentence(fs.ToString(), true);
                TMP_Dropdown.OptionData entry = new TMP_Dropdown.OptionData(j);
                fullscreenSelection.options.Add(entry);

                if (newSettings.display.fullScreen == fs) {
                    fullscreenSelection.value = (int)fs;
                }
            }
            fullscreenSelection.RefreshShownValue();
        }

        public void OnSelectFullscreen(int selection) {
            newSettings.display.fullScreen = (FullScreenMode)fullscreenSelection.value;
        }
        #endregion

        public void ApplyDisplay() {
            GlobalManager.Settings.settings.display = newSettings.display;
            GlobalManager.Settings.settings.display.Apply();
        }

        // public void ApplyAudio() {
        //     settings.audio.Apply();
        // }


        void Start() {
            if (!requested) {
                gameObject.SetActive(false);
            }
        }

        void OnEnable() {
            StartCoroutine(DelayedEnableRoutine());
            newSettings = GlobalManager.Settings.settings;

            panels = new GameObject[] {displayPanel, audioPanel};

            ConstructResolutionMenu();
            ConstructFullscreenMenu();
        }
        
        IEnumerator DelayedEnableRoutine() {
            yield return null;
            backButton.Select();
        }
        protected override void SetOpen(bool state) {
            // If we're opening the options menu, let's check to see what the
            // current UI open is, so that we can reopen it once we're done.
            if (state) {
                lastUI = CurrentUI;
            }
            base.SetOpen(state);
            if (state) {
                requested = true;
            } else {
                // If we're leaving the options menu, re-open the last UI.
                if (lastUI != null) {
                    lastUI.IsOpen = true;
                }
            }
        }
    }
}