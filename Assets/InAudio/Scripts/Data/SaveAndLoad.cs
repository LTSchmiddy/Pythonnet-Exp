using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InAudioSystem
{
public static class SaveAndLoad  
{
    private static Component[] GetComponents(GameObject go)
    {
        if (go != null)
        {
            return go.GetComponentsInChildren(typeof(MonoBehaviour), true);
        }
        return null;
    }

    public static void LoadManagerData(out Component[] audioData, out Component[] eventData, out Component[] musicData,  out Component[] bankLinkData)
    {
        
        GameObject eventDataGO      = Resources.Load(FolderSettings.EventLoadData) as GameObject;
        GameObject bankLinkDataGO   = Resources.Load(FolderSettings.BankLinkLoadData) as GameObject;
        GameObject audioDataGO      = Resources.Load(FolderSettings.AudioLoadData) as GameObject;
        GameObject musicDataGO      = Resources.Load(FolderSettings.MusicLoadData) as GameObject;

        audioData = GetComponents(audioDataGO);
        eventData = GetComponents(eventDataGO);
        bankLinkData = GetComponents(bankLinkDataGO);
        musicData = GetComponents(musicDataGO);
    }

    #if UNITY_EDITOR    
    public static void CreateDataPrefabs(GameObject AudioRoot, GameObject MusicRoot, GameObject EventRoot, GameObject BankLinkRoot)
    {
        CreateMusicRootPrefab(MusicRoot);
        CreateAudioNodeRootPrefab(AudioRoot);
        CreateAudioEventRootPrefab(EventRoot);
        CreateAudioBankLinkPrefab(BankLinkRoot);
    }

    public static void CreateAudioNodeRootPrefab(GameObject root)
    {
        // PrefabUtility.CreatePrefab(FolderSettings.AudioSaveDataPath, root);
        PrefabUtility.SaveAsPrefabAsset(root, FolderSettings.AudioSaveDataPath);
        Object.DestroyImmediate(root);
    }
    public static void CreateAudioEventRootPrefab(GameObject root)
    {
        PrefabUtility.SaveAsPrefabAsset(root, FolderSettings.EventSaveDataPath);
        Object.DestroyImmediate(root);
    }
    public static void CreateAudioBankLinkPrefab(GameObject root)
    {
        PrefabUtility.SaveAsPrefabAsset(root, FolderSettings.BankLinkSaveDataPath);
        Object.DestroyImmediate(root);
    }
    public static void CreateMusicRootPrefab(GameObject root)
    {
        PrefabUtility.SaveAsPrefabAsset(root, FolderSettings.MusicSaveDataPath);
        Object.DestroyImmediate(root);
    }


    #endif
}
}
