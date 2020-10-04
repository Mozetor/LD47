﻿using Assets.Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour {

    public int cost;
    public int damage;
    public float range;
    public float attackCooldown;
    public List<EnemyType> targets;
    public TowerProjectile projectile;
    public Transform turretHead;

    private float currentAttackCooldown = 0;


    private void Update() {
        if (currentAttackCooldown > 0) {
            currentAttackCooldown -= Time.deltaTime;
            return;
        }

        var enemies = FindObjectsOfType<Enemy>()
            .Where(HasValidType)
            .Where(IsInRange)
            .ToList();

        if (enemies.Count == 0)
            return;

        var target = enemies[0];
        StartCoroutine(Attack(target));
    }

    private bool HasValidType(Enemy enemy) => targets.Contains(enemy.enemyType);

    private bool IsInRange(Enemy enemy) => Vector3.Distance(enemy.transform.position, this.transform.position) <= range;

    private IEnumerator Attack(Enemy target) {
        currentAttackCooldown = attackCooldown;
        var forward = target.transform.position - this.transform.position;

        var startQuat = this.transform.rotation;
        var targetQuat = Quaternion.FromToRotation(Vector3.down, forward);

        for (int i = 0; i < 10; i++) {
            this.transform.rotation = Quaternion.Lerp(startQuat, targetQuat, (i + 1) / 9f);
            yield return new WaitForSeconds(0.001f);
        }

        var lifetime = range / projectile.speed * 2;

        var proj = Instantiate(projectile, this.transform.position, Quaternion.identity);
        proj.direction = forward;
        proj.damage = damage;
        proj.lifeTime = lifetime;
        proj.enemyTypes = this.targets;
    }
}