using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LetsTryEngine
{ 
    [CreateAssetMenu(fileName = "New Dictionary", menuName = "Can Be Useful/Dictionary Storage")]
    public class DictionaryScriptableObject : ScriptableObject
    {
        [SerializeField]
        List<string> keyList = new List<string>();
        [SerializeField]
        List<GameObject> valueList = new List<GameObject>();

        public List<string> KeysList { get => keyList; set => keyList = value; }
        public List<GameObject> ValueList { get => valueList; set => valueList = value; }
    }
}
