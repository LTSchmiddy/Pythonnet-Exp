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
    [CustomPropertyDrawer(typeof(PythonEvent))]
    public class PythonEventPropertyDrawer : PropertyDrawer {
        private static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float indent = 15.0f;
        private static readonly float indent2 = 15.0f;
        private static readonly float expandedLines = 19f;

        Rect storedPosition;

        public Rect UpdatedPosition(float height = 1, float spacing = 0.5f) {
            storedPosition.y += storedPosition.height + (lineHeight * spacing);
            storedPosition.height = (lineHeight * height);
            return storedPosition;

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            storedPosition = new Rect(position.x + indent2, position.y, position.width - indent2, lineHeight);

            // Move this up
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            {
                // Here we add the foldout using a single line height, the label and change
                // the value of property.isExpanded
                property.isExpanded = EditorGUI.Foldout(
                    new Rect(
                        position.x + indent, position.y, 
                        position.width - indent, 
                        lineHeight
                    ),
                    property.isExpanded,
                    label
                );

                // Now you want to draw the content only if you unfold this property
                if (property.isExpanded) {
                    EditorGUI.LabelField(UpdatedPosition(), "Import Code");
                    property.FindPropertyRelative("importCode").stringValue = EditorGUI.TextArea(
                        UpdatedPosition(height: 5, spacing: 0.2f),
                        property.FindPropertyRelative("importCode").stringValue
                    );
                    EditorGUI.LabelField(UpdatedPosition(), "Event Code");
                    property.FindPropertyRelative("eventCode").stringValue = EditorGUI.TextArea(
                        UpdatedPosition(height: 5, spacing: 0.2f),
                        property.FindPropertyRelative("eventCode").stringValue
                    );
                    EditorGUI.PropertyField(UpdatedPosition(), property.FindPropertyRelative("objectRefs"), true);



                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return lineHeight * (property.isExpanded ? expandedLines : 1) + property.FindPropertyRelative("objectRefs").arraySize * lineHeight;
        }
    }
}
