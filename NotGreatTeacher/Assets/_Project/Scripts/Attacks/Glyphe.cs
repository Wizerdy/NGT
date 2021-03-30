using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project {

    public class Glyphe : MonoBehaviour {
        [HideInInspector] public Vector2Int[] range;

        private Animator anim;

        public bool IsResolving {
            get { return anim.GetBool("Attack"); }
        }

        private void Awake() {
            anim = GetComponent<Animator>();
            if (anim == null) { Debug.LogError("No animator found : " + gameObject.name); }
        }

        public void Resolve() {
            anim.SetBool("Attack", true);
        }

        public void Attack() {
            GameManager.instance.DealDamage(range);
        }

        public void Destroy() {
            GameManager.instance.ToVoid(gameObject);
        }
    }
}