using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlexEditorUtilities.ProjectBuilder {
    public class ProjectBuilderUtilityWindow : EditorWindow {
        
        [MenuItem("Alex's Tools/Project Builder Utility")]
        public static void OpenWindow() {
            ProjectBuilderUtilityWindow window = (ProjectBuilderUtilityWindow)EditorWindow.GetWindow(typeof(ProjectBuilderUtilityWindow));

            window.Init();
            window.Show();
        }
        
        
        void Init() {
            titleContent = new GUIContent("Project Builder Utility");
        }

        void OnGUI() {
            if (GUILayout.Button("Add Python Runtime To")) {}
            if (GUILayout.Button("Build Python Scripts Archive")) {}
            if (GUILayout.Button("Build Core Player")) {}
        }
    }
}