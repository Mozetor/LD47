namespace PlayerBuilding {
    public interface IDamageable {
        /// <summary>
        /// Damages.
        /// </summary>
        void Damage(int damage);
        /// <summary>
        /// Return current health points.
        /// </summary>
        /// <returns></returns>
        int GetHealth();
    }
}