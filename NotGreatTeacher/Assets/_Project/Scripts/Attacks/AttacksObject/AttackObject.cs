using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    public abstract class AttackObject : ScriptableObject {
        public GameObject attack;
        [SerializeField] protected int damage;
        protected int touchNumber;

        #region Properties

        public int Damage {
            get { return damage; }
        }

        public int TouchNumber {
            get { return touchNumber; }
        }

        #endregion

        public AttackObject() {
            touchNumber = 0;
        }

        #region Attack range

        //public virtual Nullable<Vector2Int>[] AttackRange(int x, int y) {
        //    return null;
        //}

        //public Nullable<Vector2Int>[] AttackRange(Vector2Int coord) {
        //    return AttackRange(coord.x, coord.y);
        //}

        public virtual Nullable<Vector2Int>[] AttackRange(params Vector2Int[] coords) {
            if (coords.Length > touchNumber) { Debug.LogError("Trying to call with too much coordinates (" + name + ")"); return null; }
            if (coords.Length < touchNumber) { Debug.LogError("Trying to call with not enough coordinates (" + name + ")"); return null; }
            return coords.ToNullableArray();
        }

        public Nullable<Vector2Int>[] AttackRange(params int[] coords) {
            Vector2Int[] array = new Vector2Int[Mathf.CeilToInt(coords.Length / 2f)];
            for (int i = 0; i < array.Length; i++) {
                array[i] = new Vector2Int(coords[i * 2], coords[i * 2 + 1]);
            }

            if (coords.Length % 2 != 0) {
                array[array.Length - 1] = new Vector2Int(coords[coords.Length - 1], 0);
            }

            return AttackRange(array);
        }

        #endregion

        public GameObject IntantiateAttack(Vector3 position, Vector2Int[] range) {
            GameObject insta = Instantiate(attack, position, Quaternion.identity);
            Glyphe glyphe = insta.GetComponent<Glyphe>();
            if (glyphe == null) { Debug.LogError("No glyphe script : " + insta.name); return insta; }
            glyphe.range = range;
            return insta;
        }
    }
}
