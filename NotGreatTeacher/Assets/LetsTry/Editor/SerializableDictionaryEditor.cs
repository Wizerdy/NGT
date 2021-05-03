using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LetsTryEngine;

namespace LetsTryEditor {
    [CustomEditor(typeof(SerializableDictionary))]
    public class SerializableDictionaryEditor : Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (((SerializableDictionary)target).modifyValues)
            {
                if(GUILayout.Button("Save changes"))
                {
                    ((SerializableDictionary)target).DeserializedDictionary();
                }
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Show Dictionary"))
            {
                ((SerializableDictionary)target).PrintDictionary();
            }
        }
    }
}
