namespace Assets.Enemies {
    public class BomberEnemy : NavEnemy2D
    {
        protected override void Attack() {
            target.damageable.Damage(settings.damage);
            settings.TakeDamage(settings.maxHealth);
        }
    }
}