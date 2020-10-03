using Assets.Enemies.Behaviour;
using Assets.WaveSpawner;
using System;
using UnityEngine;

namespace Assets.Enemies {
    public class Enemy : Spawnable {

        public int maxHealth;
        public float range;
        public EnemyType enemyType;
        public float speed;
        public int damage;
        public float attackCooldown;
        public CityController cityController;
        public Paths paths;
        public float pathContinueDistance;

        private int health;
        private Action<int, int> onDamageTaken;
        private float currentAttackCooldown;
        private Path path;
        private int pathIndex;

        public void Start() {
            health = maxHealth;
            cityController = FindObjectOfType<CityController>();
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
                this.transform.position += (target - this.transform.position).normalized * speed * Time.deltaTime;
            }
        }

        public void AddOnDamageTaken(Action<int, int> onDamageTaken) {
            this.onDamageTaken += onDamageTaken;
        }

        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                health = 0;
                onDamageTaken?.Invoke(health, damage);
                Die();
            } else {
                onDamageTaken?.Invoke(health, damage);
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