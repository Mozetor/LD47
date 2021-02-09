using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour {
    public Slider healthBar;

    private void Awake() {
        var enemy = GetComponentInParent<NavEnemy2D>();
        enemy.settings.AddOnHealthUpdated(UpdateHealthBar);
    }

    private void UpdateHealthBar(int health, int maxHealth, int _) {
        healthBar.value = (float)health / maxHealth;
    }
}
