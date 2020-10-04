using Assets.Enemies.Behaviour;
using Assets.WaveSpawner;
using City;
using System;
using UnityEngine;

namespace Assets.Enemies {
    public class Enemy : Spawnable {

        private const int GROUNDED = 10;
        private const int AIRBORNE = 11;

        public int maxHealth;
        public float range;
        public EnemyType enemyType;
        public float speed;
        public int damage;
        public float attackCooldown;
        public CityController cityController;
        public Paths paths;
        public Transform graphics;
        public float pathContinueDistance;

        private int health;
        private Action<int, int, int> onHealthUpdated;
        private float currentAttackCooldown;
        private Path path;
        private int pathIndex;

        public void Start() {
            health = maxHealth;
            onHealthUpdated?.Invoke(health, maxHealth, 0);
            cityController = FindObjectOfType<CityController>();
            gameObject.layer = enemyType == EnemyType.GROUNDED ? GROUNDED : AIRBORNE;
        }

        // Update is called once per frame
        public void Update() {
            if (Input.GetKeyDown(KeyCode.K))
                Die();

            if (Vector3.Distance(this.transform.position, path[pathIndex]) < pathContinueDistance && pathIndex + 1 < path.pathPoints.Count) {
                pathIndex++;
            }

            var target = path[pathIndex];

            if (Vector3.Distance(this.transform.position, cityController.transform.position) < range) {
                currentAttackCooldown -= Time.deltaTime;
                if (currentAttackCooldown <= 0) {
                    currentAttackCooldown = attackCooldown;
                    Attack();
                }
            } else {
                var direction = (target - this.transform.position).normalized;
                this.transform.position += direction * speed * Time.deltaTime;
                graphics.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            }
        }

        public void AddOnHealthUpdated(Action<int, int, int> onHealthUpdated) {
            this.onHealthUpdated += onHealthUpdated;
        }

        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                health = 0;
                onHealthUpdated?.Invoke(health, maxHealth, damage);
                Die();
            } else {
                onHealthUpdated?.Invoke(health, maxHealth, damage);
            }
        }

        public override void SetUp(int spawnIndex) {
            path = paths.paths[spawnIndex];
            pathIndex = 0;
        }

        protected override void Die() {
            base.Die();
            Destroy(this.gameObject);
        }

        protected virtual void Attack() {
            cityController.Damage(damage);
        }
    }
}