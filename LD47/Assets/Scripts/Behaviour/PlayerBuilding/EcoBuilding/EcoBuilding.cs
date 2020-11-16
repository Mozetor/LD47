using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy;
using City;

namespace PlayerBuilding.EcoBuilding {
    public class EcoBuilding : MonoBehaviour, IPlaceable {
        /// <summary> Building name </summary>
        public new string name;
        /// <summary> Building Cost </summary>
        public BuildResource[] cost;

        /// <summary> Amount of resources added by this building </summary>
        public BuildResource resourceGenerated;


        #region IPlaceableImplementation
        public BuildResource[] GetCost() {
            return cost;
        }

        public GameObject GetObject() {
            return this.gameObject;
        }

        public void FinishPlacement(GameObject placedObject) {
            PlayerBuildingPlacer.AddBuilding(placedObject);
            FindObjectOfType<CityController>().IncreaseIncome(resourceGenerated);
        }
        public void PrepareRemoval() {
            FindObjectOfType<CityController>().DecreaseIncome(resourceGenerated);
        }

        public bool CanUpgrade() {
            throw new System.NotImplementedException();
        }

        public void Upgrade() {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}