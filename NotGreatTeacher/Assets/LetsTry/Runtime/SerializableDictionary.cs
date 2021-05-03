using UnityEngine;
using System;
using System.Collections.Generic;

namespace LetsTryEngine {
    public class SerializableDictionary : MonoBehaviour, ISerializationCallbackReceiver 
    {
        [SerializeField]
        private DictionaryScriptableObject dictionaryStorage;

        [SerializeField]
        private List<string> keysList = new List<string>();
        [SerializeField]
        private List<GameObject> valuesList = new List<GameObject>();

        private Dictionary<string, GameObject> theFamousSerializableDictionary = new Dictionary<string, GameObject>();

        public bool modifyValues;
        private void Awake()
        {
            for (int i = 0; i < Mathf.Min(dictionaryStorage.KeysList.Count, dictionaryStorage.ValueList.Count); i++)
            {
                theFamousSerializableDictionary.Add(dictionaryStorage.KeysList[i], dictionaryStorage.ValueList[i]);
            }
        }
        public void OnBeforeSerialize()
        {
            if(modifyValues == false)
            {
                keysList.Clear();
                valuesList.Clear();
                for (int i = 0; i < Mathf.Min(dictionaryStorage.KeysList.Count, dictionaryStorage.ValueList.Count); i++)
                {
                    keysList.Add(dictionaryStorage.KeysList[i]);
                    valuesList.Add(dictionaryStorage.ValueList[i]);
                }
            }
        }

        public void OnAfterDeserialize()
        {

        }

        public void DeserializedDictionary()
        {
            Debug.Log("AAAAAAAAAAAAAAAAhhhhhhhhhhhhh !!!!");
            theFamousSerializableDictionary = new Dictionary<string, GameObject>();
            dictionaryStorage.KeysList.Clear();
            dictionaryStorage.ValueList.Clear();
            for (int i = 0; i < Mathf.Min(keysList.Count,valuesList.Count); i++)
            {
                dictionaryStorage.KeysList.Add(keysList[i]);
                dictionaryStorage.ValueList.Add(valuesList[i]);
                theFamousSerializableDictionary.Add(keysList[i], valuesList[i]);
            }
            modifyValues = false;
        }

        public void PrintDictionary()
        {
            foreach (var pair in theFamousSerializableDictionary)
            {
                Debug.Log("Key: " + pair.Key + " Value: " + pair.Value);
            }
        }
    }
}