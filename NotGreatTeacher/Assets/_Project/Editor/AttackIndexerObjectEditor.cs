using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Project {
    [CustomEditor(typeof(AttackIndexerObject))]
    public class AttackIndexerObjectEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            SerializedProperty attacksProp = serializedObject.FindProperty("attacks");

            if (attacksProp == null) { return; }

            int enumSize = Enum.GetNames(typeof(GameManager.Attacks)).Length;

            float totalSize = enumSize / 2f * (enumSize + 1);

            //Debug.Log(attacksProp.arraySize + " .. " + enumSize);
            if (attacksProp.arraySize < totalSize) {
                int arraySize = attacksProp.arraySize;
                for (int i = 0; i < totalSize - arraySize; i++) {
                    attacksProp.InsertArrayElementAtIndex(attacksProp.arraySize);
                }
            }

            for (int i = 0; i < enumSize; i++) {
                EditorGUILayout.PropertyField(attacksProp.GetArrayElementAtIndex(i), new GUIContent(((GameManager.Attacks)i).ToString()));
            }

            int index = enumSize;
            for (int i = 0; i < enumSize; i++) {
                for (int j = i + 1; j < enumSize; j++) {
                    EditorGUILayout.PropertyField(attacksProp.GetArrayElementAtIndex(index), new GUIContent(((GameManager.Attacks)i).ToString() + " + " + ((GameManager.Attacks)j).ToString()));
                    index++;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}