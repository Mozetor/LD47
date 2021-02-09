using Assets.WaveSpawner;
using Buildings;
using City;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies {

    public class NavEnemy2D : Spawnable, IDamageable {

        protected enum State {
            MoveToDefaultTarget,
            MoveToTarget,
            AttackTarget
        }


        public EnemySettings settings;
        public Transform graphics;
        public ContactFilter2D filter;


        protected NavMeshAgent agent;
        protected State state;
        protected (Vector3 pos, IDamageable damageable, Collider2D collider) target;
        private (Vector3 pos, IDamageable damageable, Collider2D collider) defaultTarget;
        private Collider2D agroCollider;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.stoppingDistance = Mathf.Max(0, settings.range - 0.1f);
            agent.speed = settings.speed;
            state = State.MoveToDefaultTarget;
            var city = FindObjectOfType<CityController>();
            defaultTarget = (city.transform.position, city.GetComponent<IDamageable>(), city.GetComponent<Collider2D>());
            agroCollider = transform.GetChild(0).GetComponent<Collider2D>();
            settings.AddOnDeath(base.Die);
            settings.AddOnDeath(() => Destroy(gameObject));
            ResetTarget();
        }

        private void Update() {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
                Die();
#endif
            settings.UpdateAttackCooldown(Time.deltaTime);
            Rotate();
            switch (state) {
                case State.MoveToDefaultTarget:
                    FindPossibleTarget();
                    if (GetDistanceToTarget() < settings.range) {
                        state = State.AttackTarget;
                    }
                    break;
                case State.MoveToTarget:
                    if (GetDistanceToTarget() < settings.range) {
                        state = State.AttackTarget;
                        agent.isStopped = true;
                    }
                    break;
                case State.AttackTarget:
                    if (target.damageable == null) {
                        ResetTarget();
                    }
                    if (GetDistanceToTarget() > settings.range) {
                        state = State.MoveToTarget;
                        agent.isStopped = false;
                    }
                    else {
                        if (settings.Attack()) {
                            int healthLeft = target.damageable.GetHealth() - settings.damage;
                            Attack();
                            if (healthLeft <= 0) {
                                ResetTarget();
                            }
                        }
                    }
                    break;
            }
        }

        private void FindPossibleTarget() {
            List<Collider2D> colliders = new List<Collider2D>();
            agroCollider.OverlapCollider(filter, colliders);
            if (colliders.Count == 0) {
                return;
            }
            var foundTarget = GetTarget(colliders);
            if (foundTarget.damageable != null) {
                target = foundTarget;
                state = State.MoveToTarget;
                agent.destination = target.pos;
            }
        }

        private (Vector3 pos, IDamageable damageable, Collider2D collider) GetTarget(List<Collider2D> colliders) {
            (Vector3 pos, IDamageable damageable, Collider2D collider) target = (Vector3.zero, null, null);
            float dist = float.MaxValue;
            Vector2 myPos = GetPosition();
            foreach (var collider in colliders) {
                IDamageable currDamageable = collider.GetComponent<IDamageable>();
                float currDist = Vector2.Distance(myPos, collider.ClosestPoint(myPos));
                if (currDamageable != null && currDist < dist) {
                    target = (collider.gameObject.transform.position, currDamageable, collider);
                    dist = currDist;
                }
            }
            return target;
        }

        private void ResetTarget() {
            target = defaultTarget;
            agent.destination = defaultTarget.pos;
            state = State.MoveToDefaultTarget;
        }

        private void Rotate() {
            graphics.rotation = Quaternion.LookRotation(Vector3.forward, agent.velocity - transform.position);
        }

        private Vector2 GetPosition() => new Vector2(transform.position.x, transform.position.y);

        private float GetDistanceToTarget() => Vector2.Distance(GetPosition(), target.collider.ClosestPoint(GetPosition()));

        public void Damage(int damage) => settings.TakeDamage(damage);

        public int GetHealth() => settings.Health;

        protected virtual void Attack() => target.damageable.Damage(settings.damage);
    }
}