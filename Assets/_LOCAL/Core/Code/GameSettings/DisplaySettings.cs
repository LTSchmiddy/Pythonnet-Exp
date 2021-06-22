using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameUniverse;

namespace GameSettings {
    [System.Serializable]

    public struct DisplaySettings : IGameSettingType {
        
        public FullScreenMode fullScreen;
        public int refreshRate;
        public int width;
        public int height;

        public void SetByIndex(int i) {
            width = Screen.resolutions[i].width;
            height = Screen.resolutions[i].height;
            refreshRate = Screen.resolutions[i].refreshRate;
        }

        public string GetName() {
            return "Display";
        }

        public void Apply() {
            GlobalManager.GlobalCoroutine(ApplyCoroutine());
        }

        IEnumerator ApplyCoroutine() {
            Debug.Log("Setting Display Mode:");
            Debug.Log(width);
            Debug.Log(height);
            Debug.Log(fullScreen);
            Debug.Log(refreshRate);

            // Screen.SetResolution(width, height, fullScreen, refreshRate);
            // Screen.fullScreen = fullScreen == FullScreenMode.ExclusiveFullScreen || fullScreen == FullScreenMode.FullScreenWindow;
            Screen.fullScreenMode = fullScreen;

            // Apparently, Unity has trouble setting fullscreen and and screen resolution during the same frame.
            // Delaying the change of resolution by 2 frames seems to help.
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Screen.SetResolution(width, height, fullScreen, refreshRate);
        }
    }
}
