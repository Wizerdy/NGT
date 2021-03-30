using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InterfacesContainer {
    [CustomPropertyDrawer(typeof(InterfaceContainer<>))]
    public class InterfaceContainerDrawer : PropertyDrawer {
        private bool msgBox = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //base.OnGUI(position, property, label);
            msgBox = false;
            Rect pos = position;
            pos.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty typeProp = property.FindPropertyRelative("type");
            SerializedProperty assemblyProp = property.FindPropertyRelative("assembly");

            SerializedProperty objProp = property.FindPropertyRelative("obj");
            EditorGUI.PropertyField(pos, objProp);
            pos.y += EditorGUIUtility.singleLineHeight;

            if (objProp.objectReferenceValue == null) {
                return;
            }

            if (typeProp != null) {
                //Debug.Log(property.FindPropertyRelative("Type"));
                //Debug.Log(typeProp.stringValue);
                Component[] components = ((GameObject)objProp.objectReferenceValue).GetComponents(typeof(Component));
                Type type = Type.GetType(typeProp.stringValue + ", " + assemblyProp.stringValue);
                Debug.Log(typeProp.stringValue + " . " + (type == null) + " .. " + typeProp.stringValue + ", " + assemblyProp.stringValue);
                for (int i = 0; i < components.Length; i++) {
                    //if (components[i].GetType().ToString().Equals(typeProp.stringValue)) {
                    //    return;
                    //}
                    //Debug.Log(components[i].GetType() + " .. " + type + " .. " + components[i].GetType().IsAssignableFrom(type));
                    if (components[i].GetType().IsAssignableFrom(type)) {
                        return;
                    }
                }
                EditorGUI.HelpBox(pos, "Type not matching", MessageType.Error);
                msgBox = true;
            } else {
                EditorGUI.HelpBox(pos, "Type variable not set", MessageType.Error);
                msgBox = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            //return base.GetPropertyHeight(property, label);
            return EditorGUIUtility.singleLineHeight * (msgBox ? 2f : 1f);
        }
    }
}