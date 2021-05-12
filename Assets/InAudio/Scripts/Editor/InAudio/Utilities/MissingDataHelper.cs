﻿using System;
using InAudioSystem.ExtensionMethods;
using InAudioSystem.Internal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace InAudioSystem.InAudioEditor
{
    public class MissingDataHelper
    {
        public static void StartFromScratch(InCommonDataManager Manager)
        {
            try
            {
                DataCleanup.Cleanup(DataCleanup.CleanupVerbose.Silent);
            }
            catch (Exception)
            {
                //Do nothing as something is seriously wrong
            }


            int levelSize = 3;
            GameObject audioGO = new GameObject();
            GameObject eventGO = new GameObject();
            GameObject bankGO = new GameObject();
            GameObject musicGO = new GameObject();

            Manager.BankLinkTree = AudioBankWorker.CreateTree(bankGO);
            Manager.AudioTree = AudioNodeWorker.CreateTree(audioGO, levelSize);
            Manager.EventTree = AudioEventWorker.CreateTree(eventGO, levelSize);
            Manager.MusicTree = MusicWorker.CreateTree(musicGO, levelSize);

            SaveAndLoad.CreateDataPrefabs(Manager.AudioTree.gameObject, Manager.MusicTree.gameObject, Manager.EventTree.gameObject,
                Manager.BankLinkTree.gameObject);

            Manager.Load(true);

            if (Manager.BankLinkTree != null)
            {
                var bankLink = Manager.BankLinkTree._children[0];
                bankLink._name = "Default - Auto loaded";
                bankLink._autoLoad = true;

                NodeWorker.AssignToNodes(Manager.AudioTree, node =>
                {
                    var data = (node._nodeData as InFolderData);
                    if (data != null)
                        data.BankLink = Manager.BankLinkTree._getChildren[0];
                });

                NodeWorker.AssignToNodes(Manager.MusicTree, musicNode =>
                {
                    var folder = musicNode as InMusicFolder;
                    if (folder != null)
                        folder._bankLink = Manager.BankLinkTree._getChildren[0];
                });

                AssetDatabase.Refresh();
                DataCleanup.Cleanup(DataCleanup.CleanupVerbose.Silent);
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            else
            {
                Debug.LogError("There was a problem creating the data.");
            }
        }
    }

}