using Assets.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour {
    public Slider healthBar;

    private void Awake() {
        var enemy = GetComponentInParent<Enemy>();
        enemy.AddOnHealthUpdated(UpdateHealthBar);
    }

    private void UpdateHealthBar(int health, int maxHealth, int _) {
        healthBar.value = (float) health / maxHealth;
    }
}
