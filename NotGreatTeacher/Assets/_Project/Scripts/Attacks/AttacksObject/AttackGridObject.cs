using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    [CreateAssetMenu(fileName = "StantardAttack", menuName = "Attack/Stantard attack", order = 1)]
    public class AttackGridObject : AttackObject {
        [SerializeField] public CoordPicker range;

        public AttackGridObject() : base() {
            touchNumber = 1;
        }

        //public override Nullable<Vector2Int>[] AttackRange(int x, int y) {
        //    Vector2Int[] attackZone = range.GetRelativesCoord();
        //    Nullable<Vector2Int>[] relativesAttacks = new Nullable<Vector2Int>[attackZone.Length];

        //    for (int i = 0; i < relativesAttacks.Length; i++) {
        //        relativesAttacks[i] = new Vector2Int(
        //            attackZone[i].x + x,
        //            attackZone[i].y + y
        //        );
        //    }

        //    return relativesAttacks;
        //}

        public override Nullable<Vector2Int>[] AttackRange(params Vector2Int[] coords) {
            if (base.AttackRange(coords) == null) { return null; }

            Vector2Int[] attackZone = range.GetRelativesCoord();
            Nullable<Vector2Int>[] relativesAttacks = new Nullable<Vector2Int>[attackZone.Length];

            for (int i = 0; i < relativesAttacks.Length; i++) {
                relativesAttacks[i] = new Vector2Int(
                    attackZone[i].x + coords[0].x,
                    attackZone[i].y + coords[0].y
                );
            }

            return relativesAttacks;
        }
    }
}
