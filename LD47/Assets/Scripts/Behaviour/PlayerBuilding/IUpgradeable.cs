using Economy;

namespace PlayerBuilding
{
    public interface IUpgradeable
    {
        /// <summary>
        /// Checks if building can be upgraded.
        /// </summary>
        bool CanUpgrade();
        /// <summary>
        /// Upgrades Building to next level.
        /// </summary>
        void Upgrade();
        /// <summary>
        /// Returns true if Building can no longer be upgraded.
        /// </summary>
        bool IsMaxUpgrade();
        /// <summary>
        /// Returns current Building upgrade level.
        /// </summary>
        int GetBuildingLevel();
    }
}