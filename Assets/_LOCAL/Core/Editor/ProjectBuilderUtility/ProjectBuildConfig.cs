using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

using AlexEditorUtilities;
using PythonEngine;
using PythonEngineEditor;

namespace AlexEditorUtilities.ProjectBuilder {

    [CreateAssetMenu(menuName = "_LOCAL/Editor/Project Build Config", fileName = "new ProjectBuildConfig", order = 0)]
    public class ProjectBuildConfig : ScriptableObject {

        // Player Info
        public string buildOutputDir = "./Build/DevBuild";
        public BuildTarget buildTarget;
        public BuildTargetGroup buildTargetGroup;
        public BuildOptions buildOptions;
        public bool useEditorScenes = true;
        public SceneReference[] includedScenes;

        // SaveData Info
        public string defaultRecordsSource = "./DefaultRecords";

        // Python Info
        public string pythonRuntimeDir = "";
        public string pythonCodeArchiveName = "core.zip";

        public BuildReport BuildCorePlayer() {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            if (useEditorScenes) {
                buildPlayerOptions.scenes = new string[EditorBuildSettings.scenes.Length];
            } else {
                buildPlayerOptions.scenes = new string[includedScenes.Length];
            }
            for(int i = 0; i < buildPlayerOptions.scenes.Length; i++) {
            
                if (useEditorScenes) {
                    buildPlayerOptions.scenes[i] = EditorBuildSettings.scenes[i].path;
                } else {
                    buildPlayerOptions.scenes[i] = includedScenes[i];
                }
            }

            buildPlayerOptions.locationPathName = buildOutputDir + "/" + PlayerSettings.productName + ".exe";
            buildPlayerOptions.target = buildTarget;
            buildPlayerOptions.targetGroup = buildTargetGroup;
            buildPlayerOptions.options = buildOptions;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded) {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed) {
                Debug.Log("Build failed");
            }

            return report;
        }

        public void CopyDefaultRecordsToBuild() {
            PythonEditorUtilities.DirectoryCopy(defaultRecordsSource, buildOutputDir + "/DefaultRecords", true);
        }

        public void CopyPythonRuntimeToBuild() {
            PythonEditorUtilities.DirectoryCopy("./PythonRuntime", buildOutputDir + "/PythonRuntime", true);
        }

        public void CopyPythonScriptsToBuild() {
            PythonEditorUtilities.DirectoryCopy("./PythonScripts", buildOutputDir + "/PythonScripts", true);
        }

        public void CopyInternalPythonScriptsToBuild() {
            PythonEditorUtilities.DirectoryCopy("./PythonScripts_Internal", buildOutputDir + "/PythonScripts_Internal", true);
        }

        public void PythonCodeArchiveForBuild() {
            PythonEditorUtilities.BuildPythonCodeArchive("./Assets", buildOutputDir + "/" + pythonCodeArchiveName);
        }
    }

    [CustomEditor(typeof(ProjectBuildConfig), true)]
    public class ProjectBuildConfigEditor : AlexEditorBase<ProjectBuildConfig> {

        List<string> pythonRuntimeDirs;

        private bool showingBuildOptions = false;

        public override void OnEnable() {
            pythonRuntimeDirs = new List<string>(Directory.GetDirectories(PythonManager.PYTHON_RUNTIME_DIRECTORY));
        }

        public override void OnInspectorGUI_Easy() {
            PlayerBuildInfo();
            SaveDataBuildInfo();
            PythonBuildInfo();

        }

        public void PlayerBuildInfo() {
            bool do_build = false;
            EditorGUILayout.BeginHorizontal();
            Header("Core Player: ");
            if (GUILayout.Button("Build Core Player")) {
                do_build = true;
            }
            EditorGUILayout.EndHorizontal();

            OpenFolderPathAutoPropertyField("buildOutputDir", "./", "Build Output Location");
            AutoPropertyField("buildTargetGroup");
            AutoPropertyField("buildTarget");
            AutoPropertyField("useEditorScenes");
            if (!AutoProperty("useEditorScenes").boolValue){
                AutoPropertyField("includedScenes");
            }
            BuildOptionsSelector();
            EditorGUILayout.Space(10);
            
            if (do_build) {
                Target.BuildCorePlayer();
            }
        }

        public void BuildOptionsSelector() {
            AutoPropertyField("buildOptions");
            EditorGUI.indentLevel++;
            showingBuildOptions = EditorGUILayout.Foldout(showingBuildOptions, "Expanded View:");
            if (showingBuildOptions) {
                EditorGUI.indentLevel++;
                BuildOptions currentOptions = 0;

                foreach (BuildOptions i in Enum.GetValues(typeof(BuildOptions))) {
                    if (i == 0) {
                        continue;
                    }
                    if (EditorGUILayout.Toggle(Enum.GetName(typeof(BuildOptions), i), (Target.buildOptions & i) != 0)) {
                        currentOptions |= i;
                    }
                }

                Target.buildOptions = currentOptions;
                EditorGUI.indentLevel--;

            }

            EditorGUI.indentLevel--;
        }

        public void SaveDataBuildInfo() {
            Header("Save Data: ");
            OpenFolderPathAutoPropertyField("defaultRecordsSource", "", "DefaultRecords", new GUIContent("Default Records Source"));
            if (GUILayout.Button("Copy Default Records To Build")) {
                Target.PythonCodeArchiveForBuild();
            }
            EditorGUILayout.Space(10);
        }

        public void PythonBuildInfo() {
            Header("Python Runtime: ");
            OpenFolderPathAutoPropertyField("pythonRuntimeDir", PythonManager.PYTHON_RUNTIME_DIRECTORY, "");

            AutoPropertyField("pythonCodeArchiveName");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Runtime to Build")) {
                Target.CopyPythonRuntimeToBuild();
            }
            if (GUILayout.Button("Build Python Code Archive")) {
                Target.PythonCodeArchiveForBuild();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Internal Scripts Folder to Build")) {
                Target.CopyInternalPythonScriptsToBuild();
            } 

            if (GUILayout.Button("Copy Scripts Folder to Build")) {
                Target.CopyPythonScriptsToBuild();
            }           
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }

    }
}