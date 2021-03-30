using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    [CreateAssetMenu(fileName = "StantardAttack", menuName = "Attack/Stantard attack", order = 1)]
    public class AttackGridObject : AttackObject {
        [SerializeField] public CoordPicker range;

        public AttackGridObject() {
            type = AttackType.SINGLE;
        }

        public override Nullable<Vector2Int>[] AttackRange(int x, int y) {
            Vector2Int[] attackZone = range.GetRelativesCoord();
            Nullable<Vector2Int>[] relativesAttacks = new Nullable<Vector2Int>[attackZone.Length];

            for (int i = 0; i < relativesAttacks.Length; i++) {
                relativesAttacks[i] = new Vector2Int(
                    attackZone[i].x + x,
                    attackZone[i].y + y
                );
            }

            return relativesAttacks;
        }
    }
}
