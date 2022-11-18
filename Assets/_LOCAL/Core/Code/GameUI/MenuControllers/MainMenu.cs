using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GameUI {

    public class MainMenu : ExclusiveUI {
        private static MainMenu _instance;
        public static MainMenu Instance { get => _instance; private set => _instance = value; }
        public static void Show() {
            Instance.IsOpen = true;
        }
        public static void Hide() {
            Instance.IsOpen = false;
        }

        public Selectable NewGameButton;
        public Selectable LoadGameButton;
        public Selectable OptionsButton;
        public Selectable QuitButton;


        public MainMenu() {
            Instance = this;
        }

        void OnEnable() {
            StartCoroutine(DelayedEnableRoutine());
        }

        IEnumerator DelayedEnableRoutine() {
            yield return null;
            NewGameButton.Select();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (Instance == this) {
                Instance = null;
            }
        }


    }
}