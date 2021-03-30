using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    public class Entity : MonoBehaviour {
        private const float DESTINATION_RADIUS = 0.1f;

        public int life = 1;

        private float timeToDestination = 0f;
        private float destinationTimer = 0f;

        private Nullable<Vector2> destination = null;
        private Nullable<Vector2> basePosition = null;

        #region Properties

        public Vector2 Position {
            get { return transform.position.To2D(Axis.Z); }
            set { transform.position = value.To3D(transform.position.z, Axis.Z); }
        }

        public bool IsMoving {
            get { return !(destination == null); }
        }

        public int Life {
            get { return life; }
            set { life = value; CheckLife(); }
        }

        #endregion

        #region Unity callbacks

        private void Start() {
            //MoveTo(GameManager.instance.Grid.GetCellPosition(new Vector2Int(5, 0)), 1f);
        }

        private void FixedUpdate() {
            UpdateDestination();
        }

        #endregion

        #region Movements

        private void UpdateDestination() {
            if (destination != null) {
                destinationTimer += Time.fixedDeltaTime;
                Position = Vector2.Lerp(basePosition, destination, destinationTimer / (timeToDestination > 0f ? timeToDestination : 1f));

                if (IsNearPoint(destination, DESTINATION_RADIUS)) {
                    destination = null;
                }
            }
        }

        #endregion

        #region Movements controller

        public void MoveTo(Vector2 destination, float time) {
            this.destination = destination;
            destinationTimer = 0f;
            timeToDestination = time;
            basePosition = Position;
        }

        #endregion

        public bool IsNearPoint(Vector2 position, float radius) {
            if ((Position - position).sqrMagnitude < radius * radius) {
                return true;
            }
            return false;
        }

        public void CheckLife() {
            if (Life <= 0) {
                Die();
            }
        }

        public void Die() {
            GameManager.instance.Dying(this);
            Destroy(this.gameObject);
        }
    }
}