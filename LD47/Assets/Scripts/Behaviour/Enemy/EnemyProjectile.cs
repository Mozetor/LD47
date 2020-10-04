using City;
using UnityEngine;

namespace Assets.Enemies {
    public class EnemyProjectile : Projectile {
        protected override bool IsValidCollision(Collision2D collision) {
            var city = collision.gameObject.GetComponent<CityController>();
            return city != null;
        }

        protected override void OnValidCollision(Collision2D collision) {
            var city = collision.gameObject.GetComponent<CityController>();
            city.Damage(damage);
        }
    }
}
