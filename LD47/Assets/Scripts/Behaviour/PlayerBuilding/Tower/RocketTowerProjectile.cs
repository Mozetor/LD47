using Assets.Enemies;
using System.Linq;
using UnityEngine;

namespace Assets.Towers {
    public class RocketTowerProjectile : TowerProjectile {

        public float explosionRange;

        protected override bool IsValidCollision(Collision2D collision) => true;

        protected override void OnValidCollision(Collision2D collision) {
            var center = this.transform.position;
            var targets = FindObjectsOfType<Enemy>()
                .Where(IsInRange)
                .Where(IsOfCorrectType)
                .ToList();
            targets.ForEach(e => e.TakeDamage(damage));
        }

        private bool IsInRange(Enemy enemy) => Vector3.Distance(enemy.transform.position, this.transform.position) < this.explosionRange;
    }
}