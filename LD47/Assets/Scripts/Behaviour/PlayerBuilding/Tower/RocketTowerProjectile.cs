using System.Linq;
using UnityEngine;

namespace Assets.Towers
{
    public class RocketTowerProjectile : TowerProjectile
    {

        public float explosionRange;

        protected override bool IsValidCollision(Collision2D collision) => true;

        protected override void OnValidCollision(Collision2D collision)
        {
            var targets = FindObjectsOfType<NavEnemy2D>()
                .Where(IsInRange)
                .Where(IsOfCorrectType)
                .ToList();
            targets.ForEach(e => e.settings.TakeDamage(damage));
        }

        private bool IsInRange(NavEnemy2D enemy) => Vector3.Distance(enemy.transform.position, transform.position) < explosionRange;
    }
}