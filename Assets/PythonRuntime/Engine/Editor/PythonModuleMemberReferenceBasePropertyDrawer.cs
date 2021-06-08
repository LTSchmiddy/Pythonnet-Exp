using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using PythonEngine;
using PythonEngine.References;

namespace PythonEngineEditor {
    // IngredientDrawerUIE
    
    public abstract class PythonModuleMemberReferenceBasePropertyDrawer : PropertyDrawer {
        public abstract List<string> getMemberSearchList(PythonFile file);
        public abstract string getMemberLabel();

        Rect storedPosition;

        private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float indent = 15.0f;
        private static readonly float expandedLines = 3f;

        public Rect UpdatedPosition() { 
                storedPosition.y += lineHeight;
                return storedPosition;

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            storedPosition = new Rect(position.x + indent, position.y, position.width - indent, lineHeight);

            // Move this up
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            {
                // Here we add the foldout using a single line height, the label and change
                // the value of property.isExpanded
                property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, lineHeight), property.isExpanded, label);

                // Now you want to draw the content only if you unfold this property
                if (property.isExpanded) {
                    EditorGUI.PropertyField(UpdatedPosition(), property.FindPropertyRelative("pythonFile"));

                    PythonFile file = (PythonFile)property.FindPropertyRelative("pythonFile").objectReferenceValue;

                    if (file == null) {
                        EditorGUI.LabelField(UpdatedPosition(), "No Module");

                    } else {
                        string m_name =property.FindPropertyRelative("modulePath").stringValue;

                        SerializedProperty classNameProp = property.FindPropertyRelative("memberName");
                        // EditorGUI.PropertyField(UpdatedPosition(), property.FindPropertyRelative("className"));
                        int currentSelection = file.definedClassNames.IndexOf(classNameProp.stringValue);
                        if (currentSelection < 0) {
                            currentSelection = 0;
                        }
                        property.FindPropertyRelative("memberName").stringValue = getMemberSearchList(file)[
                                EditorGUI.Popup(
                                    UpdatedPosition(),
                                    getMemberLabel(), 
                                    currentSelection, 
                                    getMemberSearchList(file).ToArray()
                                )
                            ];
                    }
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return lineHeight * (property.isExpanded ? expandedLines : 1);
        }
    }

    [CustomPropertyDrawer(typeof(PythonClassReference))]
    public class PythonClassReferencePropertyDrawer : PythonModuleMemberReferenceBasePropertyDrawer {
        public override string getMemberLabel() {
            return "Class Name";
        }

        public override List<string> getMemberSearchList(PythonFile file) {
            return file.definedClassNames;
        }
    }

    [CustomPropertyDrawer(typeof(PythonFunctionReference))]
    public class PythonFunctionReferencePropertyDrawer : PythonModuleMemberReferenceBasePropertyDrawer {
        public override string getMemberLabel() {
            return "Function Name";
        }

        public override List<string> getMemberSearchList(PythonFile file) {
            return file.definedFunctionNames;
        }
    }
}
