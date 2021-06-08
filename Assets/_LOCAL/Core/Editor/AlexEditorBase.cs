using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AlexEditorBase<T> : Editor where T : UnityEngine.Object {

    //public T Target;

    protected T Target { get { return (T)target; } }
    protected T[] Targets { get { return System.Array.ConvertAll<UnityEngine.Object, T>(targets, item => (T)item); } }

    public Dictionary<string, SerializedProperty> MyProperties = new Dictionary<string, SerializedProperty>();

    protected GUIStyle HeaderStyle {
        get {
            GUIStyle retVal = new GUIStyle();
            retVal.fontStyle = FontStyle.Bold;
            retVal.normal.textColor = Color.white;
            return retVal;
        }
    }


    public virtual void OnEnable() {

    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        OnInspectorGUI_Easy();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void OnInspectorGUI_Easy() {
        DrawDefaultInspector();
    }

    public virtual void OnInspectorGUI_Base() {
        base.OnInspectorGUI();
    }


    public void Text(string text, params GUILayoutOption[] options) {
        EditorGUILayout.LabelField(text, options);
    }

    public void Header(string text) {
        EditorGUILayout.LabelField(text, HeaderStyle);
    }

    public SerializedProperty AutoProperty(string propertyName) {
        if (!MyProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                return null;
            }
            MyProperties.Add(propertyName, newProp);

            return newProp;
        } else {
            return MyProperties[propertyName];
        }
    }

    // Return value is whether or not the property exists. Useful for drawing compound widgets:
    public bool AutoPropertyField(string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        SerializedProperty prop = AutoProperty(propertyName);

        if (prop == null) {
            EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
            return false;
        }

        if (label == null) {
            EditorGUILayout.PropertyField(MyProperties[propertyName], includeChildren, options);
        } else {
            EditorGUILayout.PropertyField(MyProperties[propertyName], label, includeChildren, options);
        }
        return true;
    }

    public bool PositionedAutoPropertyField(Rect pos, string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        SerializedProperty prop = AutoProperty(propertyName);

        if (prop == null) {
            EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
            return false;
        }

        if (label == null) {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], includeChildren);
        } else {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], label, includeChildren);
        }
        return true;
    }

    public void OpenFolderPathAutoPropertyField(
        string propertyName,
        string folder,
        string defaultName,
        GUIContent label = null,
        params GUILayoutOption[] options
    ) {
        EditorGUILayout.BeginHorizontal(options);
        if (AutoPropertyField(propertyName)) {
            if (GUILayout.Button("Select Folder", GUILayout.MaxWidth(90f))) {
                AutoProperty(propertyName).stringValue = EditorUtility.OpenFolderPanel(
                    "Select " + (string.IsNullOrWhiteSpace(label.text) ? propertyName : label.text),
                    folder,
                    defaultName
                );
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public U UnityObjectLayoutField<U>(string Label, U obj, bool allowSceneObjects, params GUILayoutOption[] options) where U : UnityEngine.Object {

        return AlexEditorHelper.AutoObjectLayoutField<U>(Label, obj, allowSceneObjects, options);
    }

    public U UnityObjectField<U>(Rect rect, string Label, U obj, bool allowSceneObjects) where U : UnityEngine.Object {

        return AlexEditorHelper.AutoObjectField<U>(rect, Label, obj, allowSceneObjects);
    }

    public static void QuickSerializedPropertyField(UnityEngine.Object useObject, string PropertyName, bool includeChildren = false) {
        SerializedObject mySerialized = new SerializedObject(useObject);
        mySerialized.Update();
        if (includeChildren) {
            EditorGUILayout.PropertyField(mySerialized.FindProperty(PropertyName), true);
        } else {
            if (mySerialized != null) {
                try {
                    EditorGUILayout.PropertyField(mySerialized.FindProperty(PropertyName));
                } catch {
                    EditorGUILayout.LabelField("Could not access property");
                }
            }
        }
        mySerialized.ApplyModifiedProperties();
    }
}

public class AlexEditorHelper {
    public static U AutoObjectLayoutField<U>(string Label, U obj, bool allowSceneObjects, params GUILayoutOption[] options) where U : UnityEngine.Object {

        return (U)EditorGUILayout.ObjectField(Label, obj, typeof(U), allowSceneObjects, options);
    }

    public static U AutoObjectField<U>(Rect rect, string Label, U obj, bool allowSceneObjects) where U : UnityEngine.Object {

        return (U)EditorGUI.ObjectField(rect, Label, obj, typeof(U), allowSceneObjects);
    }

    public static string StringPopupLayout(string label, string selection, List<string> choices) {
        int currentSelection = choices.IndexOf(selection);
        if (currentSelection < 0) {
            currentSelection = 0;
        }
        return choices[
            EditorGUILayout.Popup(
                label,
                currentSelection,
                choices.ToArray()
            )
        ];
    }

    public static string StringPopup(Rect rect, string label, string selection, List<string> choices) {
        int currentSelection = choices.IndexOf(selection);
        if (currentSelection < 0) {
            currentSelection = 0;
        }
        return choices[
            EditorGUI.Popup(
                rect,
                label,
                currentSelection,
                choices.ToArray()
            )
        ];
    }
}

[System.Serializable]
public class AutoPropertyManager {

    protected SerializedObject serializedObject;
    public Dictionary<string, SerializedProperty> myProperties = new Dictionary<string, SerializedProperty>();



    public AutoPropertyManager(SerializedObject pSObject) {
        serializedObject = pSObject;

    }

    public SerializedObject GetSObject() {
        return serializedObject;
    }

    public bool WasObjectLost() {
        return serializedObject == null;
    }

    public void Update() {
        serializedObject.Update();
    }
    public void Apply() {
        serializedObject.ApplyModifiedProperties();
    }

    public SerializedProperty AutoProperty(string propertyName) {
        if (!myProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                return null;
            }
            myProperties.Add(propertyName, newProp);
        }

        return myProperties[propertyName];
    }

    //SerializedProperty property, GUIContent label, bool includeChildren, params GUILayoutOption[] options);
    public void LayoutAutoPropertyField(string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        SerializedProperty prop = AutoProperty(propertyName);

        if (prop == null) {
            EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
            return;
        }

        if (label == null) {
            EditorGUILayout.PropertyField(myProperties[propertyName], includeChildren, options);
        } else {
            EditorGUILayout.PropertyField(myProperties[propertyName], label, includeChildren, options);
        }

    }

    public void AutoPropertyField(Rect pos, string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        SerializedProperty prop = AutoProperty(propertyName);

        if (prop == null) {
            EditorGUI.LabelField(pos, "Property \"" + propertyName + "\" Does Not Exist");
            return;
        }

        if (label == null) {
            EditorGUI.PropertyField(pos, prop, includeChildren);
        } else {
            EditorGUI.PropertyField(pos, prop, label, includeChildren);
        }

    }

    public static void QuickSerializedPropertyField(UnityEngine.Object useObject, string PropertyName, bool includeChildren = false) {
        SerializedObject mySerialized = new SerializedObject(useObject);
        mySerialized.Update();
        if (includeChildren) {
            EditorGUILayout.PropertyField(mySerialized.FindProperty(PropertyName), true);
        } else {
            if (mySerialized != null) {
                try {
                    EditorGUILayout.PropertyField(mySerialized.FindProperty(PropertyName));
                } catch {
                    EditorGUILayout.LabelField("Could not access property");
                }
            }

        }

        mySerialized.ApplyModifiedProperties();
    }




}