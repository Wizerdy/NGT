using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    //[CreateAssetMenu(fileName = "StantardAttack", menuName = "Attack/Multiple attack", order = 1)]
    public abstract class AttackMultipleObject : AttackObject {
        public AttackMultipleObject() : base() {
            touchNumber = 2;
        }

        public override Nullable<Vector2Int>[] AttackRange(params Vector2Int[] coords) {
            if (coords.Length > touchNumber) { Debug.LogError("Trying to call with too much coordinates (" + name + ")"); return null; }
            return coords.ToNullableArray();
        }

        //public override Nullable<Vector2Int>[] AttackRange(int x, int y) {
        //    Debug.LogError("Trying to call without enough coordinates (" + name + ")");
        //    return null;
        //}
    }
}
