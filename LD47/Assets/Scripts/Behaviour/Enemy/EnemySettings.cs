using Assets.Enemies;
using System;
using UnityEngine;

[Serializable]
public class EnemySettings
{
    public string enemyName;
    public EnemyType enemyType;
    public int maxHealth;
    public float range;
    public float speed;
    public int damage;
    public float attackCooldown;
    private int health;
    private Action<int, int, int> onHealthUpdated;
    private float currAttackCooldown;
    private Action onDeath;

    public int Health => health;

    public EnemySettings()
    {
        health = maxHealth;
    }

    public void UpdateAttackCooldown(float dtime)
    {
        currAttackCooldown = Mathf.Max(0, currAttackCooldown - dtime);
    }

    public bool Attack()
    {
        if (currAttackCooldown == 0)
        {
            currAttackCooldown = attackCooldown;
            return true;
        }
        return false;
    }

    public void AddOnHealthUpdated(Action<int, int, int> onHealthUpdated) => this.onHealthUpdated += onHealthUpdated;

    public void AddOnDeath(Action onDeath) => this.onDeath += onDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            onHealthUpdated?.Invoke(health, maxHealth, damage);
            onDeath?.Invoke();
        }
        else
        {
            onHealthUpdated?.Invoke(health, maxHealth, damage);
        }
    }
}
