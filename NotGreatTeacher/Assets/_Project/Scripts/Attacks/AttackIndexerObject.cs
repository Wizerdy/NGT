using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {
    [CreateAssetMenu(fileName = "AttackIndexer", menuName = "Attack/Attack indexer", order = 0)]
    public class AttackIndexerObject : ScriptableObject {
        public AttackObject[] attacks = new AttackObject[4];

        public AttackObject GetAttack(int index) {
            return attacks[index % attacks.Length];
        }
    }
}