using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemies {
    public class ProjectileEnemy : Enemy {

        public Projectile projectile;

        protected override void Attack() {
            var forward = cityController.transform.position - this.transform.position;
            var lifetime = range / projectile.speed * 2;

            var proj = Instantiate(projectile, this.transform.position, Quaternion.identity);
            proj.direction = forward;
            proj.damage = damage;
            proj.lifeTime = lifetime;
        }
    }
}
