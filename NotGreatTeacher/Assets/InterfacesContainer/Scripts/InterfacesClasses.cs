using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterfacesContainer {
    //[Serializable]
    //public class InterfacesClasses : InterfaceContainer<ITest> {

    //}

    public class InterfacesClasses : MonoBehaviour {
        public InterfaceContainer<ITest> test;
    }

    public interface ITest {

    }

    [Serializable]
    [ExecuteInEditMode]
    public class InterfaceContainer<T> where T : class {
        public GameObject obj = null;
        public string type = "";
        public string assembly = "";

        public string Type {
            get { assembly = typeof(T).Assembly.GetName().ToString(); return type = typeof(T).ToString(); }
        }

        public InterfaceContainer() {
            //Debug.LogWarning(Type + " . " + assembly);
            Debug.LogWarning(typeof(T).Assembly.FullName.ToString());
        }

        public T Interface {
            get {
                if (obj is T inter) {
                    return inter;
                }
                return null;
            }
        }
    }
}