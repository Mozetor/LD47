using Economy;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBuilding
{
    public interface IPlaceable<T>
    {
        /// <summary>
        /// Returns building name.
        /// </summary>
        /// <returns>Cost</returns>
        string GetName();
        /// <summary>
        /// Returns cost.
        /// </summary>
        /// <returns>Cost</returns>
        BuildResource[] GetCost();
        /// <summary>
        /// Returns the gameObject from this building.
        /// </summary>
        /// <returns>Object</returns>
        GameObject GetObject();
        ///// <summary>
        ///// 
        ///// </summary>
        //void FinishPlacement();
        /// <summary>
        /// Prepare object Removal, works as controlled onDestroy.
        /// </summary>
        void PrepareRemoval();
        /// <summary>
        /// Returns a list of all position offsets this building will be placed on.
        /// </summary>
        /// <returns> List of offsets </returns>
        List<Vector2Int> GetOffsetPositions();
        /// <summary>
        /// Returns the thing to place.
        /// </summary>
        /// <returns></returns>
        T GetT();
    }
}