using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {
    [Serializable]
    public class CoordPicker {
        public Vector2Int[] coordinates;
        public Vector2Int center;

        public Vector2Int[] GetRelativesCoord() {
            Vector2Int[] relativseAttacks = new Vector2Int[coordinates.Length];

            for (int i = 0; i < relativseAttacks.Length; i++) {
                relativseAttacks[i] = new Vector2Int(
                    coordinates[i].x - center.x,
                    coordinates[i].y - center.y
                );
            }

            return relativseAttacks;
        }
    }
}
