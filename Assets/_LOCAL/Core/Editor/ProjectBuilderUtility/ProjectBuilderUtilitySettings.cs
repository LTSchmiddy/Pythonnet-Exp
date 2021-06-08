using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AlexEditorUtilities;

namespace AlexEditorUtilities.ProjectBuilder {
    
    public class ProjectBuilderUtilitySettings : CustomProjectSettingsBase<ProjectBuilderUtilitySettings> {
        // Overrides the default 'GetSettingsName' method:
        public static new string GetSettingsName() {return "ProjectBuilderUtilitySettings";}

        public ProjectBuildConfig[] configs;
        
        public override void InitialValues() {
            configs = new ProjectBuildConfig[1];
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class ProjectBuilderUtilitySettingsRegister {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/ProjectBuilderUtilitySettings", SettingsScope.Project) {
                // By default the last token of the path is used as display name if no label is provided.
                label = "Project Builder Utility Settings",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) => {
                    var settings = ProjectBuilderUtilitySettings.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("configs"), new GUIContent("Configurations"));
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Number", "Some String" })
            };

            return provider;
        }
    }

}