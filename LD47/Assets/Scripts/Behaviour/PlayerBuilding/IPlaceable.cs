using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBuilding {
    public interface IPlaceable {
        /// <summary>
        /// Returns cost.
        /// </summary>
        /// <returns>Cost</returns>
        int GetCost();
        /// <summary>
        /// Returns the gameObject from this building.
        /// </summary>
        /// <returns>Object</returns>
        GameObject GetObject();
        /// <summary>
        /// Finish up placement preperations, works as controlled start.
        /// </summary>
        void FinishPlacement(GameObject placedObject);
        /// <summary>
        /// Prepare object Removal, works as controlled onDestroy.
        /// </summary>
        void PrepareRemoval();
        /// <summary>
        /// Checks if building can be upgraded.
        /// </summary>
        bool CanUpgrade();
        /// <summary>
        /// Upgrades Building to next level.
        /// </summary>
        void Upgrade();
    }
}