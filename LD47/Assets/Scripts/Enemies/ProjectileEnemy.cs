using UnityEngine;

namespace Enemies
{
    public class ProjectileEnemy : NavEnemy2D
    {

        public EnemyProjectile projectile;

        protected override void Attack()
        {
            var forward = target.pos - transform.position;
            var lifetime = settings.range / projectile.speed * 2;

            var proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.direction = forward;
            proj.damage = settings.damage;
            proj.lifeTime = lifetime;
        }
    }
}
