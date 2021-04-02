using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using UnityEngine;

namespace Project {
    public class SlashObject : AttackObject {
        public SlashObject() : base() {
            touchNumber = 2;
        }

        public override Nullable<Vector2Int>[] AttackRange(params Vector2Int[] coords) {
            if (base.AttackRange(coords) == null) { return null; }

            int magx = coords[1].x - coords[0].x;
            int magy = coords[1].y - coords[0].y;

            if (Mathf.Abs(magx) != Mathf.Abs(magy) || (magx == 0 || magy == 0)) { return null; }

            int rangeLength = Mathf.Max(Mathf.Abs(magx), Mathf.Abs(magy));
            Nullable<Vector2Int>[] range = new Nullable<Vector2Int>[rangeLength];

            Vector2Int delta = new Vector2Int(
                magx == 0 ? 0 : 1 * (int)Mathf.Sign(magx),
                magy == 0 ? 0 : 1 * (int)Mathf.Sign(magy)
            );

            for (int i = 0; i < range.Length; i++) {
                range[i] = coords[0] + delta;
            }

            return range;
        }
    }
}
