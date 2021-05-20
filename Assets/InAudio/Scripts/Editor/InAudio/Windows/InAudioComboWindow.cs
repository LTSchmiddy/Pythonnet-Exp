using System;
using InAudioSystem.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

#if !UNITY_5_2
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif 


namespace InAudioSystem.InAudioEditor
{
    public class InAudioComboWindow : InAudioBaseWindow
    {

        private static Color SELECTED_WINDOW_COLOR = new Color(0.2f, 0.2f, 0.2f);

        public enum InAudioWindowMode {
            Audio,
            Music,
            Events,
            Aux
        }

        public InAudioWindowMode CurrentWindowMode = InAudioWindowMode.Audio;

        //Audio Window Variables:
        private AudioCreatorGUI audioCreatorGUI;

        //Music Window Variables:
        private MusicCreatorGUI musicCreatorGUI;


        //Event Window Variables:
        private AudioEventCreatorGUI audioEventCreatorGUI;


        //Aux Window Variables:
        private int selectedToolbar = 0;
        private readonly string[] toolbarOptions = { "Banks", "Integrity", "Project Data" };

        private AudioMixerGroup selectedBus;

        private AudioBankCreatorGUI bankGUI;
        private IntegrityGUI integrityGUI;

        // GUIStyle selectedWindowStyle = new GUIStyle();


        //Methods:
        private void OnEnable()
        {
            BaseEnable();

            if (audioCreatorGUI == null)
            {
                audioCreatorGUI = new AudioCreatorGUI(this);

            }
            audioCreatorGUI.OnEnable();


            if (musicCreatorGUI == null) {
                musicCreatorGUI = new MusicCreatorGUI(this);
                autoRepaintOnSceneChange = true;
            }
            musicCreatorGUI.OnEnable();

            if (audioEventCreatorGUI == null) {
                audioEventCreatorGUI = new AudioEventCreatorGUI(this);
            }
            audioEventCreatorGUI.OnEnable();


            if (bankGUI == null)
                bankGUI = new AudioBankCreatorGUI(this);
            if (integrityGUI == null)
                integrityGUI = new IntegrityGUI(this);
            bankGUI.OnEnable();
            integrityGUI.OnEnable();
        }


        [MenuItem("Alex's Tools/InAudio Combo Window")]
        public static InAudioComboWindow Launch()
        {
            InAudioComboWindow window = EditorWindow.GetWindow <InAudioComboWindow>();
            window.Show();

            //window.minSize = new Vector2(800, 200);
            window.titleContent = new GUIContent("InAudio DB");
            return window;
        }

        private GameObject cleanupGO;

        private void Update() {
            BaseUpdate();

            if (cleanupGO == null) {
                //cleanupGO = Resources.Load("PrefabGO") as GameObject;
                //DontDestroyOnLoad(cleanupGO);
            }


            if (audioCreatorGUI != null && Manager != null) { 
                audioCreatorGUI.OnUpdate();
            }


            if (musicCreatorGUI != null && Manager != null) {
                musicCreatorGUI.OnUpdate();
            }

            if (audioEventCreatorGUI != null && Manager != null) {
                audioEventCreatorGUI.OnUpdate();
            }

            bankGUI.OnUpdate();
        }

        private void OnGUI() {
            DrawWindowSelector();
            // CurrentWindowMode = (InAudioWindowMode)EditorGUILayout.EnumPopup(CurrentWindowMode);
            switch (CurrentWindowMode) {
                case InAudioWindowMode.Audio:
                    DrawAudioGUI();
                    break;
                case InAudioWindowMode.Music:
                    DrawMusicGUI();
                    break;
                case InAudioWindowMode.Events:
                    DrawEventGUI();
                    break;
                case InAudioWindowMode.Aux:
                    DrawAuxGUI();
                    break;
                default:
                    DrawAudioGUI();
                    break;

            }
        }

        void DrawWindowSelector(){            
            EditorGUILayout.BeginHorizontal();
            foreach (InAudioWindowMode i in Enum.GetValues(typeof(InAudioWindowMode))) {
                Color oldColor = GUI.backgroundColor;
                
                if (CurrentWindowMode == i){
                    GUI.backgroundColor = SELECTED_WINDOW_COLOR;
                }

                if (GUILayout.Button(i.ToString())) {
                    CurrentWindowMode = i;
                }
                
                GUI.backgroundColor = oldColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        #region AudioMethods
        public void Find(Func<InAudioNode, bool> filter) {

            audioCreatorGUI.FindAudio(filter);
        }

        public void Find(InAudioNode toFind) {
            if (InAudioInstanceFinder.Instance != null)
                audioCreatorGUI.Find(toFind);
            else {
                Debug.LogError("InAudio: Cannot open window without having the manager in the scene");
            }
        }



        private void DrawAudioGUI() {
            CheckForClose();

            //int nextControlID = GUIUtility.GetControlID(FocusType.Passive) + 1;
            //Debug.Log(nextControlID);  
            if (!HandleMissingData())
            {
                return;
            }

            if (audioCreatorGUI == null)
                audioCreatorGUI = new AudioCreatorGUI(this);

            isDirty = false;


            try
            {
                DrawTop(topHeight);
                isDirty |= audioCreatorGUI.OnGUI(LeftWidth, (int) position.height - topHeight);
            }
            catch (ExitGUIException e)
            {
                throw e;
            }
                /*catch (ArgumentException e)
        {
            throw e;
        }*/
            catch (Exception e)
            {
                if (e.GetType() != typeof (ArgumentException))
                {
                    Debug.LogException(e);

                    //While this catch was made to catch persistent errors,  like a missing null check, it can also catch other errors
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.HelpBox(
                        "An exception is getting caught while trying to draw this window.\nPlease report this bug to InAudio and if possible how to reproduce it",
                        MessageType.Error);

                    EditorGUILayout.TextArea(e.ToString());
                    EditorGUILayout.EndVertical();
                }
            }


            if (isDirty)
                Repaint();

            PostOnGUI();
        }

        #endregion

        #region MusicMethods

        public void Find(Func<InMusicNode, bool> filter) {
            musicCreatorGUI.FindAudio(filter);
        }

        public void Find(InMusicNode toFind) {
            if (InAudioInstanceFinder.Instance != null)
                musicCreatorGUI.Find(toFind);
            else {
                Debug.LogError("InAudio: Cannot open window without having the manager in the scene");
            }
        }

        private void DrawMusicGUI() {
            CheckForClose();

            if (!HandleMissingData()) {
                return;
            }

            if (musicCreatorGUI == null)
                musicCreatorGUI = new MusicCreatorGUI(this);

            isDirty = false;


            try {
                DrawTop(topHeight);
                isDirty |= musicCreatorGUI.OnGUI(LeftWidth, (int)position.height - topHeight);
            } catch (ExitGUIException e) {
                throw e;
            }
            /*catch (ArgumentException e)
    {
        throw e;
    }*/
            catch (Exception e) {
                if (e.GetType() != typeof(ArgumentException)) {
                    Debug.LogException(e);

                    //While this catch was made to catch persistent errors,  like a missing null check, it can also catch other errors
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.HelpBox(
                        "An exception is getting caught while trying to draw this window.\nPlease report this bug to inaudio@outlook.com and if possible how to reproduce it",
                        MessageType.Error);

                    EditorGUILayout.TextArea(e.ToString());
                    EditorGUILayout.EndVertical();
                }
            }


            if (isDirty)
                Repaint();

            PostOnGUI();
        }
        #endregion

        #region EventMethods

        public void ReceiveNode(InMusicGroup group) {
            audioEventCreatorGUI.ReceiveNode(group);
        }

        public void ReceiveNode(InAudioNode node) {
            audioEventCreatorGUI.ReceiveNode(node);
        }

        private void DrawEventGUI() {
            CheckForClose();
            if (!HandleMissingData()) {
                return;
            }


            if (audioEventCreatorGUI == null)
                audioEventCreatorGUI = new AudioEventCreatorGUI(this);

            isDirty = false;
            DrawTop(0);

            isDirty |= audioEventCreatorGUI.OnGUI(LeftWidth, (int)position.height - topHeight);

            if (isDirty)
                Repaint();

            PostOnGUI();
        }
        #endregion

        #region AuxMethods

        private void DrawAuxGUI() {
            bankGUI.BaseOnGUI();
            CheckForClose();
            if (Manager == null) {
                Manager = InAudioInstanceFinder.DataManager;
                if (Manager == null) {
                    ErrorDrawer.MissingAudioManager();
                }
            }
            if (Manager != null) {
                bool missingaudio = Manager.AudioTree == null;
                bool missingaudioEvent = Manager.EventTree == null;
                bool missingBank = Manager.BankLinkTree == null;
                bool missingMusic = Manager.MusicTree == null;

                bool areAllMissing = missingaudio && missingaudioEvent && missingBank && missingMusic;
                bool areAnyMissing = missingaudio || missingaudioEvent || missingBank || missingMusic;

                if (areAllMissing) {
                    ErrorDrawer.AllDataMissing(Manager);
                    return;
                } else if (areAnyMissing) {
                    DrawMissingDataCreation();
                    return;
                }

            } else {
                return;
            }

            isDirty = false;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.EndVertical();
            selectedToolbar = GUILayout.Toolbar(selectedToolbar, toolbarOptions);

            if (selectedToolbar == 0) {
                isDirty |= bankGUI.OnGUI(LeftWidth, (int)position.height - (int)EditorGUIUtility.singleLineHeight);
            }

            if (selectedToolbar == 1)
                isDirty |= integrityGUI.OnGUI();

            if (selectedToolbar == 2) {
                DrawMissingDataCreation();

                DrawStartFromScratch();
            }

            if (isDirty)
                Repaint();

            PostOnGUI();
        }

        private void DrawMissingDataCreation() {
            bool missingaudio = Manager.AudioTree == null;
            bool missingaudioEvent = Manager.EventTree == null;
            bool missingbankLink = Manager.BankLinkTree == null;
            bool missingMusic = Manager.MusicTree == null;


            bool areAnyMissing = missingaudio | missingaudioEvent | missingbankLink | missingMusic;
            if (areAnyMissing) {
                string missingAudioInfo = missingaudio ? "Audio Data\n" : "";
                string missingEventInfo = missingaudioEvent ? "Event Data\n" : "";
                string missingBankInfo = missingbankLink ? "BankLink Data\n" : "";
                string missingMusicInfo = missingMusic ? "Music Data\n" : "";

                EditorGUILayout.BeginVertical();
                EditorGUILayout.HelpBox(missingAudioInfo + missingEventInfo + missingMusicInfo + missingBankInfo + "is missing.\nThis may be because of new project data is required in this version of InAudio",
                    MessageType.Error, true);

                bool areAllMissing = missingaudio && missingaudioEvent && missingbankLink && missingMusic;
                if (!areAllMissing) {

                    if (GUILayout.Button("Create missing content", GUILayout.Height(30))) {
                        int levelSize = 3;
                        //How many subfolders by default will be created. Number is only a hint for new people
                        if (missingaudio)
                            CreateAudioPrefab(levelSize);
                        if (missingaudioEvent)
                            CreateEventPrefab(levelSize);
                        if (missingbankLink)
                            CreateBankLinkPrefab();
                        if (missingMusic)
                            CreateMusicPrefab(levelSize);

                        Manager.Load(true);

                        if (Manager.AudioTree != null && Manager.BankLinkTree != null)
                            NodeWorker.AssignToNodes(Manager.AudioTree, node => {
                                var data = (node._nodeData as InFolderData);
                                if (data != null)
                                    data.BankLink = Manager.BankLinkTree._getChildren[0];
                            });
                        if (Manager.MusicTree != null && Manager.BankLinkTree != null)
                            NodeWorker.AssignToNodes(Manager.MusicTree, node => {
                                var folder = (node as InMusicFolder);
                                if (folder != null)
                                    folder._bankLink = Manager.BankLinkTree._getChildren[0];
                            });

#if !UNITY_5_2
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
                        EditorApplication.MarkSceneDirty();
                        EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif

                    }
                }
                DrawStartFromScratch();
                EditorGUILayout.EndVertical();
            }

        }

        private bool AreAllMissing() {
            bool missingaudio = Manager.AudioTree == null;
            bool missingaudioEvent = Manager.EventTree == null;
            bool missingbankLink = Manager.BankLinkTree == null;
            bool missingMusic = Manager.MusicTree == null;

            return missingaudio && missingaudioEvent && missingbankLink && missingMusic;
        }

        private void DrawStartFromScratch() {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Start over from scratch", GUILayout.Height(30))) {
                if (AreAllMissing() ||
                    EditorUtility.DisplayDialog("Create new project?", "This will delete ALL data!",
                        "Start over from scratch", "Do nothing")) {
                    MissingDataHelper.StartFromScratch(Manager);
                }
            }
        }



        public void SelectBankCreation() {
            selectedToolbar = 0;
        }

        public void SelectIntegrity() {
            selectedToolbar = 1;
        }

        public void SelectDataCreation() {
            selectedToolbar = 2;
        }

        public void FindBank(InAudioBankLink bankLink) {
            selectedToolbar = 0;
            bankGUI.Find(bankLink);

        }

        private void CreateEventPrefab(int levelSize) {
            GameObject go = new GameObject();
            Manager.EventTree = AudioEventWorker.CreateTree(go, levelSize);
            SaveAndLoad.CreateAudioEventRootPrefab(go);
        }

        private void CreateBankLinkPrefab() {
            GameObject go = new GameObject();
            Manager.BankLinkTree = AudioBankWorker.CreateTree(go);
            SaveAndLoad.CreateAudioBankLinkPrefab(go);
        }

        private void CreateMusicPrefab(int levelSize) {
            GameObject go = new GameObject();
            Manager.MusicTree = MusicWorker.CreateTree(go, levelSize);
            SaveAndLoad.CreateMusicRootPrefab(go);
        }


        private void CreateAudioPrefab(int levelSize) {
            GameObject go = new GameObject();
            Manager.AudioTree = AudioNodeWorker.CreateTree(go, levelSize);
            SaveAndLoad.CreateAudioNodeRootPrefab(go);
        }
    

    #endregion


        private void DrawTop(int topHeight)
        {
            EditorGUILayout.BeginVertical(GUILayout.Height(topHeight));
            EditorGUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            if (InAudioInstanceFinder.Instance != null &&
                InAudioInstanceFinder.Instance.GetComponent<AudioSource>() != null)
                InAudioInstanceFinder.Instance.GetComponent<AudioSource>().clip = null;
        }

        private class FileModificationWarning : UnityEditor.AssetModificationProcessor
        {
            private static string[] OnWillSaveAssets(string[] paths)
            {
                if (InAudioInstanceFinder.Instance != null)
                    InAudioInstanceFinder.Instance.GetComponent<AudioSource>().clip = null;
                return paths;
            }
        }
    }

}