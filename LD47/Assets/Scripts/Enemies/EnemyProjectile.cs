﻿using Buildings;
using Buildings.Towers;
using UnityEngine;

namespace Enemies {
    public class EnemyProjectile : Projectile {
        protected override bool IsValidCollision(Collision2D collision) {
            var city = collision.gameObject.GetComponent<IDamageable>();
            return city != null;
        }

        protected override void OnValidCollision(Collision2D collision) {
            var city = collision.gameObject.GetComponent<IDamageable>();
            city.Damage(damage);
        }
    }
}
