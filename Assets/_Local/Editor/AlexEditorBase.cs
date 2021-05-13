using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AlexEditorBase<T> : Editor where T : UnityEngine.Object {

    //public T Target;

    protected T Target { get { return (T)target; } }
    protected T[] Targets { get { return System.Array.ConvertAll<UnityEngine.Object, T>(targets, item => (T)item); } }

    public Dictionary<string, SerializedProperty> MyProperties = new Dictionary<string, SerializedProperty>();



    public virtual void OnEnable() {
    //    //Target = (T)target;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        OnInspectorGUI_Easy();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void OnInspectorGUI_Easy() {}

    public virtual void OnInspectorGUI_Base() {
        base.OnInspectorGUI();
    }


    public void Text(string text, params GUILayoutOption[] options) {
        EditorGUILayout.LabelField(text, options);
    }
    

        //SerializedProperty property, GUIContent label, bool includeChildren, params GUILayoutOption[] options);
    public void AutoPropertyField(string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        if (!MyProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
                return;
            }
            


            MyProperties.Add(propertyName, newProp);
            
        }

        if (label == null) {
            EditorGUILayout.PropertyField(MyProperties[propertyName], includeChildren, options);
        } else {
            EditorGUILayout.PropertyField(MyProperties[propertyName], label, includeChildren, options);
        }
        
    }
    
    public void PositionedAutoPropertyField(Rect pos, string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        if (!MyProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
                return;
            }

            MyProperties.Add(propertyName, newProp);
            
        }

        if (label == null) {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], includeChildren);
        } else {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], label, includeChildren);
        }
        
    }
    
    public U UnityObjectField<U>(string Label, U obj, bool allowSceneObjects, params GUILayoutOption[] options) where U : UnityEngine.Object {

        return AutoObjectField<U>(Label, obj, allowSceneObjects, options);
        //return (U)EditorGUILayout.ObjectField(Label, obj, typeof(U), allowSceneObjects, options);
    }

    public static U AutoObjectField<U>(string Label, U obj, bool allowSceneObjects, params GUILayoutOption[] options) where U : UnityEngine.Object {

        return (U)EditorGUILayout.ObjectField(Label, obj, typeof(U), allowSceneObjects, options);
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



[System.Serializable]
public class AutoPropertyManager {

    protected SerializedObject serializedObject;
    public Dictionary<string, SerializedProperty> MyProperties = new Dictionary<string, SerializedProperty>();
    
    

    public AutoPropertyManager(SerializedObject pSObject) {
        serializedObject = pSObject;
        
    }
    
    //SerializedProperty property, GUIContent label, bool includeChildren, params GUILayoutOption[] options);
    public void AutoPropertyField(string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        if (!MyProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
                return;
            }
            


            MyProperties.Add(propertyName, newProp);
            
        }

        if (label == null) {
            EditorGUILayout.PropertyField(MyProperties[propertyName], includeChildren, options);
        } else {
            EditorGUILayout.PropertyField(MyProperties[propertyName], label, includeChildren, options);
        }
        
    }
    
    public void PositionedAutoPropertyField(Rect pos, string propertyName, bool includeChildren = true, GUIContent label = null, params GUILayoutOption[] options) {
        if (!MyProperties.ContainsKey(propertyName)) {
            SerializedProperty newProp = serializedObject.FindProperty(propertyName);

            if (newProp == null) {
                EditorGUILayout.LabelField("Property \"" + propertyName + "\" Does Not Exist");
                return;
            }

            MyProperties.Add(propertyName, newProp);
            
        }

        if (label == null) {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], includeChildren);
        } else {
            EditorGUI.PropertyField(pos, MyProperties[propertyName], label, includeChildren);
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