using Assets.Enemies;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : Projectile
{

    public List<EnemyType> enemyTypes;

    protected override bool IsValidCollision(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<NavEnemy2D>();
        return enemy != null && IsOfCorrectType(enemy);
    }

    protected override void OnValidCollision(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<NavEnemy2D>();
        enemy.settings.TakeDamage(damage);
    }

    protected bool IsOfCorrectType(NavEnemy2D enemy) => enemyTypes.Contains(enemy.settings.enemyType);
}
