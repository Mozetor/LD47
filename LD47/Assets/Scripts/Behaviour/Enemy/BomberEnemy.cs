namespace Assets.Enemies {
    public class BomberEnemy : Enemy {
        protected override void Attack() {
            cityController.Damage(damage);
            TakeDamage(maxHealth);
        }
    }
}