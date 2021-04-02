//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using ToolsBoxEngine;

namespace Project {
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour {
        private enum GameState { COMBAT, ANIMATION, ATTACKS_ANIMATION }

        public enum Attacks { FIRE, LIGHTNING, SLASH, IMPACT }

        private const int TRIES_BEFORE_PASS = 4;

        public static GameManager instance;

        [SerializeField] private int healthMax = 10;
        private int health = 10;
        [SerializeField] private int movesMax = 4;
        private int movesRemaining = 0;

        private GameState gameState = GameState.ANIMATION;
        private Coroutine delayChangeState = null;

        [Header("Grid")]
        public Vector2Int gridSize = Vector2Int.zero;
        [HideInInspector] public Vector2 gridStartPosition = Vector2.zero;
        [HideInInspector] public Vector2 gridEndPosition = Vector2.zero;
        private Grid grid = null;

        [Header("Enemies")]
        public Transform enemySpawn;
        public Transform enemyParent;
        public GameObject enemy;
        private List<Entity> enemies = new List<Entity>();

        public Vector2Int enemiesNumber = new Vector2Int(10, 15);
        [SerializeField] private float enemySpeed = 1f;

        [Header("Attack")]
        private List<Vector2> attackPoints = new List<Vector2>();
        public AttackObject actualAttack = null;
        public AttackIndexerObject attackIndexer = null;
        public Transform attackParent = null;

        [Header("Other")]
        public Transform bin = null;

        #region Properties

        public int Health {
            get { return health; }
            set { SetHealth(value); }
        }

        public int MovesRemaining {
            get { return movesRemaining; }
            set {
                movesRemaining = value;
                if (movesRemaining > movesMax) movesRemaining = movesMax;
                if (movesRemaining < 0) movesRemaining = 0;
            }
        }

        public Grid Grid {
            get { return grid; }
        }

        #endregion

        #region Unity callbacks

        private void Awake() {
            instance = this;

            grid = new Grid(gridStartPosition, gridEndPosition, gridSize);

            enemies = new List<Entity>();
            attackPoints = new List<Vector2>();
        }

        private void Start() {
            gameState = GameState.ANIMATION;
            SpawnEnemies(UnityEngine.Random.Range(enemiesNumber.x, enemiesNumber.y));
            DelayChangeGameState(GameState.COMBAT, enemySpeed);

            MovesRemaining = movesMax;
            SetActualAttack(Attacks.LIGHTNING);
            //actualAttack = attackIndexer.attacks[(int)Attacks.LIGHTNING];
        }

        private void Update() {
            switch (gameState) {
                case GameState.COMBAT:
                    if (Input.GetButtonDown("Fire1")) {
                        if (actualAttack != null && !Grid.IsOutOfBounds(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
                            attackPoints.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                            if (actualAttack.TouchNumber == attackPoints.Count) {
                                bool attacked = Attack(actualAttack, attackPoints);
                                if (attacked) {
                                    MovesRemaining -= 1;
                                    if (MovesRemaining <= 0) {
                                        ChangeGameState(GameState.ATTACKS_ANIMATION);
                                    }
                                }
                            } else if (attackPoints.Count > actualAttack.TouchNumber) {
                                attackPoints.Clear();
                            }
                        } else {
                            attackPoints.Clear();
                        }

                        /*bool attacked = Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (attacked) {
                            MovesRemaining -= 1;
                            if (MovesRemaining <= 0) {
                                ChangeGameState(GameState.ATTACKS_ANIMATION);
                            }
                        }*/
                    }
                    break;
                case GameState.ANIMATION:
                    break;
                case GameState.ATTACKS_ANIMATION:
                    bool noGlyphe = ResolveAttacks();

                    if (noGlyphe) {
                        ChangeGameState(GameState.COMBAT);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Spawn enemies

        public void SpawnEnemies(int number) {
            Debug.Log(number);
            for (int i = 0; i < number; i++) {
                int tries = 0;
                Entity spawned = null;

                do {
                    spawned = SpawnEnemy(enemy, UnityEngine.Random.Range(0, gridSize.x), UnityEngine.Random.Range(0, gridSize.y));
                    tries++;
                }
                while (spawned == null && tries <= TRIES_BEFORE_PASS);

                if(tries > TRIES_BEFORE_PASS) {
                    Debug.LogWarning("Passed : limit of tries reached");
                }
            }
        }

        public Entity SpawnEnemy(GameObject enemy, int x, int y) {
            if (!Grid.IsEmpty(x, y)) { /*Debug.LogWarning("Cell already filled");*/ return null; }

            Entity entity = SpawnEnemy(enemy, enemySpawn.position.To2D());
            entity.MoveTo(Grid.GetCellPosition(x, y), enemySpeed);
            Grid.Fill(x, y, entity.gameObject);

            return entity;
        }

        public Entity SpawnEnemy(GameObject enemy, Vector2 position) {
            GameObject instance = Instantiate(enemy, position, Quaternion.identity);
            instance.transform.parent = enemyParent;
            enemies.Add(instance.GetComponent<Entity>());
            return instance.GetComponent<Entity>();
        }

        #endregion

        #region Attacks

        private bool Attack(AttackObject attack, List<Vector2> positions) {
            if (attack == null) { return false; }

            List<Nullable<Vector2Int>> ncoords = new List<Nullable<Vector2Int>>();
            for (int i = 0; i < positions.Count; i++) {
                 ncoords.Add(Grid.GetCellCoord(positions[i]));
                if (ncoords[i] == null || Grid.IsOutOfBounds(ncoords[i])) { return false; }
            }

            Vector2Int[] coords = ncoords.ToList().ToArray();

            Nullable<Vector2Int>[] range = actualAttack.AttackRange(coords);
            if (range == null) { return false; }

            //DealDamange(range.ToArray());
            actualAttack.IntantiateAttack(Grid.GetCellPosition(coords[0]).To3D(), range.ToArray()).transform.parent = attackParent;

            //TO DO : Instantiate avec plusieurs points

            return true;
        }

        private bool ResolveAttacks() {
            if (attackParent.childCount <= 0) { return true; }

            Glyphe glyphe = attackParent.GetChild(0).GetComponent<Glyphe>();

            if (glyphe != null) {
                if (!glyphe.IsResolving) {
                    glyphe.Resolve();
                }
            } else {
                Debug.LogError("Wtf bro ?");
            }

            return false;
        }

        public void DealDamage(Vector2Int[] range) {
            for (int i = 0; i < range.Length; i++) {
                int x = range[i].x,
                    y = range[i].y;
                if (!Grid.IsOutOfBounds(x, y) && !Grid.IsEmpty(x, y)) {
                    Cell cell = Grid.GetCell(x, y);
                    Entity entityInCell = cell.inside.GetComponent<Entity>();
                    if (entityInCell != null) {
                        entityInCell.Life -= actualAttack.Damage;
                    }
                }
            }
        }

        #endregion

        #region Getters

        public bool IsEnemiesMoving() {
            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i].IsMoving) {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Setters

        public void Restart() {
            health = healthMax;
            MovesRemaining = movesMax;
        }

        private void SetHealth(int amount) {
            health = amount;
        }

        private void ChangeGameState(GameState state) {
            gameState = state;
        }

        private void DelayChangeGameState(GameState state, float delay) {
            if(delayChangeState != null) { StopCoroutine(delayChangeState); }

            delayChangeState = StartCoroutine(Delay(ChangeGameState, state, delay));
        }

        public void Dying(Entity entity) {
            enemies.Remove(entity);
        }

        public void SetActualAttack(Attacks attack) {
            SetActualAttack((int)attack);
        }

        public void SetActualAttack(int attack) {
            actualAttack = attackIndexer.GetAttack(attack);
        }

        public void ToVoid(GameObject obj) {
            obj.transform.parent = bin;
            Destroy(obj);
        }

        #endregion

        #region Coroutines

        private IEnumerator Delay<T>(Tools.BasicDelegate<T> function, T arg, float time) {
            yield return new WaitForSeconds(time);
            function(arg);
        }

        #endregion

        private void OnGUI() {
            GUILayout.Label("Moves : " + MovesRemaining);
        }
    }
}