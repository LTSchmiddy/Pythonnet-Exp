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

        public enum SettingsPanel {
            Display = 0, 
            Audio = 1
        }

        [Header("Category Selection Buttons")]
        public Selectable backButton;
        public Selectable displayCategoryButton;
        public Selectable audioCategoryButton;

        [Header("Display Settings")]
        public GameObject displayPanel;
        public TMP_Dropdown displaySelection;
        public TMP_Dropdown fullscreenSelection;

        [Header("Audio Panel")]
        public GameObject audioPanel;
        public Slider masterVolSlider;
        public Slider musicVolSlider;
        public Slider guiVolSlider;
        public Slider sfxVolSlider;
        public Slider environVolSlider;

        [Header("New Settings")]
        public Settings newSettings;

        // Runtime Values
        private ExclusiveUI _lastUI { get; set; }
        private bool _requested = false;
        private GameObject[] _allPanels;
        private SettingsPanel _currentPanelId = SettingsPanel.Display;

        public GameObject[] AllPanels {
            get {
                if (_allPanels == null || _allPanels.Length < 1) {
                    _allPanels = new GameObject[] {displayPanel, audioPanel};
                    // _allPanels = new GameObject[] {audioPanel, displayPanel};
                }

                return _allPanels;
            }
        }

        public SettingsPanel CurrentPanelId {
            get {
                return _currentPanelId;
            }
            set {
                SetCurrentPanel(value);
            }
        }
        public OptionsMenu() {
            Instance = this;
        }


        void Start() {
            SetAudioSliders();
            if (!_requested) {
                gameObject.SetActive(false);
            }
        }

        void OnEnable() {
            StartCoroutine(DelayedEnableRoutine());
            newSettings = GlobalManager.Settings.settings;

            ConstructResolutionMenu();
            ConstructFullscreenMenu();
        }
        
        IEnumerator DelayedEnableRoutine() {
            yield return null;
            backButton.Select();
            
            CurrentPanelId = SettingsPanel.Display;

        }
        protected override void SetOpen(bool state) {
            // If we're opening the options menu, let's check to see what the
            // current UI open is, so that we can reopen it once we're done.
            if (state) {
                _lastUI = CurrentUI;
            }
            base.SetOpen(state);
            if (state) {
                _requested = true;
            } else {
                // If we're leaving the options menu, re-open the last UI.
                if (_lastUI != null) {
                    _lastUI.IsOpen = true;
                }
            }
        }

        public void SetCurrentPanel(int panelId) {
            SetCurrentPanel((SettingsPanel)panelId);
        }
        public void SetCurrentPanel(SettingsPanel panelId) {
            for (int i = 0; i < AllPanels.Length; i++ ) {
                AllPanels[i].SetActive(i ==(int)panelId);
            }

            _currentPanelId = panelId;
        }

        #region Display Settings Methods
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
        public void ApplyDisplay() {
            GlobalManager.Settings.settings.display = newSettings.display;
            GlobalManager.Settings.settings.display.Apply();
        }
        #endregion

        #region Audio Settings Methods

        public void SetAudioSliders() {
            masterVolSlider.value = newSettings.audio.masterVolume;
            masterVolSlider.onValueChanged.AddListener((float value) => {
                newSettings.audio.masterVolume = value;
            });

            musicVolSlider.value = newSettings.audio.musicVolume;
            musicVolSlider.onValueChanged.AddListener((float value) => {
                newSettings.audio.musicVolume = value;
            });

            guiVolSlider.value = newSettings.audio.guiVolume;
            guiVolSlider.onValueChanged.AddListener((float value) => {
                newSettings.audio.guiVolume = value;
            });

            sfxVolSlider.value = newSettings.audio.sfxVolume;
            sfxVolSlider.onValueChanged.AddListener((float value) => {
                newSettings.audio.sfxVolume = value;
            });

            environVolSlider.value = newSettings.audio.environVolume;
            environVolSlider.onValueChanged.AddListener((float value) => {
                newSettings.audio.environVolume = value;
            });
        }
        
        #endregion
    }
}