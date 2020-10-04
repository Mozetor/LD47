using Assets.Enemies;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : Projectile {

    public List<EnemyType> enemyTypes;

    protected override bool IsValidCollision(Collision2D collision) {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        return enemy != null && IsOfCorrectType(enemy);
    }

    protected override void OnValidCollision(Collision2D collision) {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.TakeDamage(damage);
    }

    protected bool IsOfCorrectType(Enemy enemy) => enemyTypes.Contains(enemy.enemyType);
}
