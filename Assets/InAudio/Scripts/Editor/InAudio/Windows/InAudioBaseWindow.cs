using InAudioSystem.ExtensionMethods;
using InAudioSystem.Internal;
using UnityEditor;
using UnityEngine;

namespace InAudioSystem.InAudioEditor
{

    public abstract class InAudioBaseWindow : EditorWindow
    {
        protected InCommonDataManager Manager;

        protected int topHeight = 0;
        protected int LeftWidth = 350;

        protected bool isDirty;

        protected void BaseEnable()
        {
            //autoRepaintOnSceneChange = true;
            EditorApplication.modifierKeysChanged += Repaint;

            Manager = InAudioInstanceFinder.DataManager;
            minSize = new Vector2(400, 100);

            EditorResources.Reload();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (OnScriptReloaded != null)
            {
                OnScriptReloaded();
            }
        }



        public void RepaintNextFrame()
        {
            
        }

        public delegate void OnScriptReloadedDelegate();

        public static event OnScriptReloadedDelegate OnScriptReloaded;

        protected void BaseUpdate()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.ValidateCommand)
                {
                    switch (Event.current.commandName)
                    {
                        case "UndoRedoPerformed":
                            Repaint();
                            break;
                    }
                }
            }
        }

        protected void CheckForClose()
        {
            if (Event.current.IsKeyDown(KeyCode.W) && Event.current.modifiers == EventModifiers.Control)
            {
                Close();
                Event.current.UseEvent();
            }
        }

        protected bool HandleMissingData()
        {
            if (InAudioInstanceFinder.InAudioGuiUserPrefs == null)
            {
                ErrorDrawer.MissingGuiUserPrefs();
                return false;
            }

            if (Manager == null)
            {
                Manager = InAudioInstanceFinder.DataManager;
                if (Manager == null)
                {
                    ErrorDrawer.MissingAudioManager();
                }
            }

            if (Manager != null)
            {
                bool areAnyMissing = ErrorDrawer.IsDataMissing(Manager);

                if (areAnyMissing)
                {
                    Manager.Load();
                }
                if (ErrorDrawer.IsAllDataMissing(Manager))
                {
                    ErrorDrawer.AllDataMissing(Manager);
                    return false;
                }
                if (ErrorDrawer.IsDataMissing(Manager))
                {
                    ErrorDrawer.MissingData(Manager);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }

        protected void PostOnGUI()
        {
            
        }

        protected bool IsKeyDown(KeyCode code)
        {
            return Event.current.type == EventType.KeyDown && Event.current.keyCode == code;
        }

    }

}